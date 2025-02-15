// SPDX-FileCopyrightText: 2022 Demerzel Solutions Limited
// SPDX-License-Identifier: LGPL-3.0-only

using System;
using System.Text;
using System.Text.Json;

using Nethermind.Core.Crypto;
using Nethermind.Core.Extensions;

namespace Nethermind.Abi
{
    public class AbiBytes : AbiType
    {
        private const int MaxLength = 32;
        private const int MinLength = 0;

        public static new AbiBytes Bytes32 { get; } = new(32);

        public AbiBytes(int length)
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(length, MaxLength);
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(length, MinLength);

            Length = length;
            Name = $"bytes{Length}";
        }

        public int Length { get; }

        public override string Name { get; }

        public override (object, int) Decode(byte[] data, int position, bool packed)
        {
            return (data.Slice(position, Length), position + (packed ? Length : MaxLength));
        }

        public override byte[] Encode(object? arg, bool packed)
        {
            if (arg is byte[] input)
            {
                if (input.Length != Length)
                {
                    throw new AbiException(AbiEncodingExceptionMessage);
                }

                return input.PadRight(packed ? Length : MaxLength);
            }

            if (arg is string stringInput)
            {
                return Encode(Encoding.ASCII.GetBytes(stringInput), packed);
            }

            if (arg is JsonElement element && element.ValueKind == JsonValueKind.String)
            {
                return Encode(Encoding.ASCII.GetBytes(element.GetString()!), packed);
            }

            if (arg is Hash256 hash && Length == 32)
            {
                return Encode(hash.Bytes.ToArray(), packed);
            }

            throw new AbiException(AbiEncodingExceptionMessage);
        }

        public override Type CSharpType { get; } = typeof(byte[]);
    }
}
