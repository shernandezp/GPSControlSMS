// Copyright (c) 2025 Sergio Hernandez. All rights reserved.
//
//  Licensed under the Apache License, Version 2.0 (the "License").
//  You may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//

using GPSControlSMS.Data;

namespace GPSControlSMS.Utils;

/// <summary>
/// Provides extension methods for command-related operations.
/// </summary>
public static class CommandExtensions
{
    /// <summary>
    /// Retrieves a list of commands associated with the specified device name.
    /// </summary>
    /// <param name="deviceName">The name of the device.</param>
    /// <returns>A list of <see cref="CommandModel"/> objects associated with the device name.</returns>
    public static List<CommandModel> GetCommands(this string deviceName)
    {
        if (CommandData.Data.TryGetValue(deviceName, out var commands))
        {
            return commands;
        }
        return [];
    }
}
