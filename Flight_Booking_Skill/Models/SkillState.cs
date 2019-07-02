// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Luis;

namespace Flight_Booking_Skill.Models
{
    public class SkillState
    {
        public string Token { get; internal set; }

        public Flight_Booking_SkillLuis LuisResult { get; internal set; }

        public void Clear()
        {
        }
    }
}
