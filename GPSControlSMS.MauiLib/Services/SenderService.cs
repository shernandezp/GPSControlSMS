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
/// Service for sending SMS messages.
/// </summary>
public class SenderService(IDisplayService displayService, ILocalizationResourceManager localization) : ISenderService
{
    /// <summary>
    /// Sends an SMS message to the specified phone number.
    /// </summary>
    /// <param name="phoneNumber">The phone number to send the SMS to.</param>
    /// <param name="message">The message content of the SMS.</param>
    public async Task SendSMSAsync(string phoneNumber, string message)
    {
        try
        {
            var smsMessage = new SmsMessage(message, phoneNumber);
            await Sms.ComposeAsync(smsMessage);
        }
        catch (FeatureNotSupportedException)
        {
            var errorMessage = localization["SmsNotSupported"];
            await displayService.ShowAlert("Error", errorMessage, "Ok");
        }
        catch (Exception ex)
        {
            var errorMessage = $"{localization["SmsError"]} {ex.Message}";
            await displayService.ShowAlert("Error", errorMessage, "Ok");
        }
    }
}
