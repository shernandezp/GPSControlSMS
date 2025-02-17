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
using GPSControlSMS.Interfaces;
using GPSControlSMS.Utils;

namespace GPSControlSMS.Services;

/// <summary>
/// Service class for managing units.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="UnitService"/> class.
/// </remarks>
/// <param name="storageService">The storage service to be used for unit management.</param>
public class UnitService(IStorageService storageService) : IUnitService
{

    /// <summary>
    /// Adds a new unit or edits an existing unit.
    /// </summary>
    /// <param name="unit">The unit to add or edit.</param>
    public void AddEditUnit(UnitModel unit)
    {
        var units = storageService.GetList<UnitModel>(Constants.UnitStorageKey).ToList();
        var existingUnit = units.FirstOrDefault(d => d.Name == unit.Name);

        if (existingUnit != null)
        {
            // Edit existing unit
            existingUnit.PhoneNumber = unit.PhoneNumber;
            existingUnit.Password = unit.Password;
            existingUnit.Device = unit.Device;
        }
        else
        {
            // Add new unit
            units.Add(unit);
        }

        storageService.SetList(Constants.UnitStorageKey, units);
    }


    /// <summary>
    /// Retrieves the list of units.
    /// If no units are found, a default unit is created.
    /// </summary>
    /// <returns>A collection of <see cref="UnitModel"/>.</returns>
    public IEnumerable<UnitModel> GetUnits()
    {
        var units = storageService.GetList<UnitModel>(Constants.UnitStorageKey);
        if (units.Any())
            return units;

        return [GetDefault()];
    }

    /// <summary>
    /// Retrieves a unit by name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public UnitModel GetUnit(string name)
    {
        var units = GetUnits().ToList();
        var unit = units.FirstOrDefault(x => x.Name.Equals(name));
        if (unit is not default(UnitModel))
            return unit;
        return GetDefault();
    }

    private static UnitModel GetDefault() 
        => new(Constants.DefaultUnit, string.Empty, Constants.Devices.ElementAt(0));

}
