#if NETSTANDARD2_0
// Trimmed-down version based on the .NET-Foundation-distributed HashCode implementation
//  which is based on the xxHash32 implementation
//    which is based on the code published by Yann Collet (Cyan4973).

/*

The xxHash32 implementation is based on the code published by Yann Collet:
https://raw.githubusercontent.com/Cyan4973/xxHash/5c174cfa4e45a42f94082dc0d4539b39696afea1/xxhash.c

  xxHash - Fast Hash algorithm
  Copyright (C) 2012-2016, Yann Collet

  BSD 2-Clause License (http://www.opensource.org/licenses/bsd-license.php)

  Redistribution and use in source and binary forms, with or without
  modification, are permitted provided that the following conditions are
  met:

  * Redistributions of source code must retain the above copyright
  notice, this list of conditions and the following disclaimer.
  * Redistributions in binary form must reproduce the above
  copyright notice, this list of conditions and the following disclaimer
  in the documentation and/or other materials provided with the
  distribution.

  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
  "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
  LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
  A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
  OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
  SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
  LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
  DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
  THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
  (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
  OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

  You can contact the author at :
  - xxHash homepage: http://www.xxhash.com
  - xxHash source repository : https://github.com/Cyan4973/xxHash

*/

using System.Runtime.CompilerServices;


namespace Org.Grush.Lib.RecordCollections;

internal struct HashCode
{
  private static readonly uint s_seed = GenerateGlobalSeed();

  private const uint Prime1 = 2654435761U;
  private const uint Prime2 = 2246822519U;
  private const uint Prime3 = 3266489917U;
  private const uint Prime4 = 668265263U;
  private const uint Prime5 = 374761393U;

  private uint _v1, _v2, _v3, _v4;
  private uint _queue1, _queue2, _queue3;
  private uint _length;

  private static uint GenerateGlobalSeed()
  {
    byte[] bytes = new byte[4];
    new Random().NextBytes(bytes);
    return BitConverter.ToUInt32(bytes, 0);
  }

  public void Add<T>(T value, IEqualityComparer<T>? comparer = null)
  {
    Add(value is null ? 0 : (comparer?.GetHashCode(value) ?? value.GetHashCode()));
  }

  private void Add(int value)
  {
    uint val = (uint)value;

    // Storing the value of _length locally shaves of quite a few bytes
    // in the resulting machine code.
    uint previousLength = _length++;
    uint position = previousLength % 4;

    // Switch can't be inlined.

    if (position == 0)
      _queue1 = val;
    else if (position == 1)
      _queue2 = val;
    else if (position == 2)
      _queue3 = val;
    else // position == 3
    {
      if (previousLength == 3)
        Initialize(out _v1, out _v2, out _v3, out _v4);

      _v1 = Round(_v1, _queue1);
      _v2 = Round(_v2, _queue2);
      _v3 = Round(_v3, _queue3);
      _v4 = Round(_v4, val);
    }
  }

  public int ToHashCode()
  {
    // Storing the value of _length locally shaves of quite a few bytes
    // in the resulting machine code.
    uint length = _length;

    // position refers to the *next* queue position in this method, so
    // position == 1 means that _queue1 is populated; _queue2 would have
    // been populated on the next call to Add.
    uint position = length % 4;

    // If the length is less than 4, _v1 to _v4 don't contain anything
    // yet. xxHash32 treats this differently.

    uint hash = length < 4 ? MixEmptyState() : MixState(_v1, _v2, _v3, _v4);

    // _length is incremented once per Add(Int32) and is therefore 4
    // times too small (xxHash length is in bytes, not ints).

    hash += length * 4;

    // Mix what remains in the queue

    // Switch can't be inlined right now, so use as few branches as
    // possible by manually excluding impossible scenarios (position > 1
    // is always false if position is not > 0).
    if (position > 0)
    {
      hash = QueueRound(hash, _queue1);
      if (position > 1)
      {
        hash = QueueRound(hash, _queue2);
        if (position > 2)
          hash = QueueRound(hash, _queue3);
      }
    }

    hash = MixFinal(hash);
    return (int)hash;
  }


  #region impl

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static void Initialize(out uint v1, out uint v2, out uint v3, out uint v4)
  {
    v1 = s_seed + Prime1 + Prime2;
    v2 = s_seed + Prime2;
    v3 = s_seed;
    v4 = s_seed - Prime1;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static uint Round(uint hash, uint input)
  {
    return BitOperations.RotateLeft(hash + input * Prime2, 13) * Prime1;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static uint QueueRound(uint hash, uint queuedValue)
  {
    return BitOperations.RotateLeft(hash + queuedValue * Prime3, 17) * Prime4;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static uint MixState(uint v1, uint v2, uint v3, uint v4)
  {
    return BitOperations.RotateLeft(v1, 1) + BitOperations.RotateLeft(v2, 7) + BitOperations.RotateLeft(v3, 12) + BitOperations.RotateLeft(v4, 18);
  }

  private static uint MixEmptyState()
  {
    return s_seed + Prime5;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static uint MixFinal(uint hash)
  {
    hash ^= hash >> 15;
    hash *= Prime2;
    hash ^= hash >> 13;
    hash *= Prime3;
    hash ^= hash >> 16;
    return hash;
  }

  private static class BitOperations
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint RotateLeft(uint value, int offset)
      => (value << offset) | (value >> (32 - offset));
  }

  #endregion
}

#endif
