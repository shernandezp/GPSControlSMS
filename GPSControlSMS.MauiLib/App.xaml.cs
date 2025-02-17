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

using System.Globalization;

namespace GPSControlSMS;

public partial class App : Application
{
    private readonly IServiceProvider _services;

    public App(IServiceProvider services)
    {
        InitializeComponent();
        _services = services;

        UserAppTheme = PlatformAppTheme;
        var culture = CultureInfo.CurrentCulture;
        var uiCulture = CultureInfo.CurrentUICulture;

        // Set the application's culture to the parent language if it's a specific culture
        if (!culture.IsNeutralCulture)
        {
            culture = culture.Parent;
        }
        if (!uiCulture.IsNeutralCulture)
        {
            uiCulture = uiCulture.Parent;
        }

        // Set the application's culture to match the device's culture
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = uiCulture;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(_services.GetRequiredService<MainPage>())
        {
            Title = "GPS Control SMS"
        };
        return window;
    }
}
