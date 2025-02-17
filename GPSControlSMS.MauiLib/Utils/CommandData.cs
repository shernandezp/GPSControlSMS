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

// I know hardcoding here is not the best practice, but for the sake of simplicity, I will hardcode the commands for each device
public static class CommandData
{
    // Dictionary to store command data for each device type
    public static readonly Dictionary<string, List<CommandModel>> Data = new()
    {
        {
            "BoxTrack", new List<CommandModel>
            {
                new("GoogleLink", "smslink******"),
                new("AccOff", "stop******"),
                new("AccOn", "resume******"),
                new("AccAlarmOn", "acc******"),
                new("AccAlarmOff", "noacc******"),
                new("ExtPowerAlarmOn", "extpower****** on"),
                new("ExtPowerAlarmOff", "extpower****** off"),
                new("Movement50", "move****** 0050"),
                new("Movement100", "move****** 0100"),
                new("Movement200", "move****** 0200"),
                new("MovementOff", "nomove******"),
                new("Reset", "reset******"),
                new("Factory", "begin******")
            }
        },
        { 
            "Coban", new List<CommandModel>() 
            {
                new("Movement", "move******"),
                new("Status", "check******"),
            }
        },
        { 
            "Concox", new List<CommandModel>()
            {
                new("GoogleLink", "URL#"),
                new("AccOff", "RELAY,1#"),
                new("AccOn", "RELAY,0#"),
                new("Reset", "REBOOT#"),
                new("Factory", "FACTORY#"),
            }
        },
        { 
            "Teltonika", new List<CommandModel>()
            {
                new("GoogleLink", "******ggps"),
                new("AccOff", "******setdigout 1"),
                new("AccOn", "******setdigout 0"),
                new("Reset", "******cpureset"),
            }
        }
    };
}
