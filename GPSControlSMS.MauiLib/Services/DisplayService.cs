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

namespace GPSControlSMS.Services;

/// <summary>
/// Service for displaying alerts in the application.
/// </summary>
public class DisplayService : IDisplayService
{
    /// <summary>
    /// Shows an alert pop up with the specified title, message, and cancel button text.
    /// </summary>
    /// <param name="title">The title of the alert.</param>
    /// <param name="message">The message of the alert.</param>
    /// <param name="cancel">The text for the cancel button.</param>
    public async Task ShowAlert(string title, string message, string cancel)
    {
        var mainPage = Application.Current.Windows[0].Page;
        if (mainPage != null)
        {
            await mainPage.DisplayAlert(title, message, cancel);
        }
    }

    /// <summary>
    /// Shows a confirmation alert with the specified title, message, accept button text, and cancel button text.
    /// </summary>
    /// <param name="title">The title of the alert.</param>
    /// <param name="message">The message of the alert.</param>
    /// <param name="accept">The text for the accept button.</param>
    /// <param name="cancel">The text for the cancel button.</param>
    /// <returns>A boolean value indicating whether the user accepted or canceled.</returns>
    public async Task<bool> ShowConfirmationAlert(string title, string message, string accept, string cancel)
    {
        var mainPage = Application.Current.Windows[0].Page;
        if (mainPage != null)
        {
            return await mainPage.DisplayAlert(title, message, accept, cancel);
        }
        return false;
    }
}
