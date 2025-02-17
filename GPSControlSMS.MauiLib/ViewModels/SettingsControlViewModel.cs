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

namespace GPSControlSMS.ViewModels;


/// <summary>
/// ViewModel for managing settings control.
/// </summary>
public class SettingsControlViewModel(
    IStorageService storageService,
    IDisplayService displayService,
    ILocalizationResourceManager localization)
{
    /// <summary>
    /// Resets the settings to their default values.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ResetToDefaultValues()
    {
        // Clear all stored settings.
        storageService.Clear();

        // Display an alert to inform the user that settings have been reset.
        var message = localization["AppReset"];
        await displayService.ShowAlert("Reset", message, "OK");
    }
}
