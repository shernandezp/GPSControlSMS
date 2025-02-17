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

using GPSControlSMS.Interfaces;
using System.Text.Json;

namespace GPSControlSMS.Services;

/// <summary>
/// Service for storing and retrieving lists of data using preferences.
/// </summary>
public class StorageService : IStorageService
{
    /// <summary>
    /// Retrieves a list of data from preferences.
    /// </summary>
    /// <typeparam name="T">The type of data to retrieve.</typeparam>
    /// <param name="key">The key used to store the data.</param>
    /// <returns>An IEnumerable of the data retrieved.</returns>
    public IEnumerable<T> GetList<T>(string key)
    {
        var value = Preferences.Default.Get(key, string.Empty);
        if (string.IsNullOrEmpty(value))
        {
            return [];
        }
        return JsonSerializer.Deserialize<IEnumerable<T>>(value) ?? [];
    }

    /// <summary>
    /// Stores a list of data in preferences.
    /// </summary>
    /// <typeparam name="T">The type of data to store.</typeparam>
    /// <param name="key">The key used to store the data.</param>
    /// <param name="value">The data to store.</param>
    public void SetList<T>(string key, IEnumerable<T> value)
    {
        var serializedValue = JsonSerializer.Serialize(value);
        Preferences.Default.Set(key, serializedValue);
    }

    
    /// <summary>
    /// Clears all stored preferences.
    /// </summary>
    public void Clear()
    {
        Preferences.Default.Clear();
    }
}
