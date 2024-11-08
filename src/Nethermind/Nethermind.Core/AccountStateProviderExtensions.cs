// SPDX-FileCopyrightText: 2022 Demerzel Solutions Limited
// SPDX-License-Identifier: LGPL-3.0-only

namespace Nethermind.Core
{
    public static class AccountStateProviderExtensions
    {
        public static bool HasCode(this IAccountStateProvider stateProvider, Address address) =>
            stateProvider.TryGetAccount(address, out AccountStruct account) && account.HasCode;
    }
}
