// https://docs.microsoft.com/en-us/visualstudio/modeling/t4-include-directive?view=vs-2017
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Bot.Builder.Solutions.Responses;

namespace Flight_Booking_Skill.Responses.FlightBooking
{
    /// <summary>
    /// Contains bot responses.
    /// </summary>
    public class FlightBookingSharedResponses : IResponseIdCollection
    {
        // Generated accessors
        public const string Booking = "Booking";
        public const string DidntUnderstandMessage = "DidntUnderstandMessage";
        public const string DidntUnderstandMessageIgnoringInput = "DidntUnderstandMessageIgnoringInput";
        public const string CancellingMessage = "CancellingMessage";
        public const string NoAuth = "NoAuth";
        public const string AuthFailed = "AuthFailed";
        public const string ActionEnded = "ActionEnded";
        public const string ErrorMessage = "ErrorMessage";
        public const string BookFlightFlowStartMessage = "BookFlightFlowStartMessage";
        public const string BookFlightDestinationSelectionPrompt = "BookFlightDestinationSelectionPrompt";
        public const string BookFlightDestinationSelectionEcho = "BookFlightDestinationSelectionEcho";
        public const string BookFlightAttendeePrompt = "BookFlightAttendeePrompt";
        public const string BookFlightDatePrompt = "BookFlightDatePrompt";
        public const string BookFlightTimePrompt = "BookFlightTimePrompt";
        public const string BookFlightDateTimeEcho = "BookFlightDateTimeEcho";
        public const string BookFlightConfirmationPrompt = "BookFlightConfirmationPrompt";
        public const string BookFlightAcceptedMessage = "BookFlightAcceptedMessage";
        public const string BookFlightFlightSearching = "BookFlightFlightSearching";
        public const string BookFlightFlightSelectionPrompt = "BookFlightFlightSelectionPrompt";
        public const string BookFlightBookingPlaceSelectionEcho = "BookFlightBookingPlaceSelectionEcho";
        public const string DestinationSelectionErrorMessage = "DestinationSelectionErrorMessage";
        public const string BookFlightFlightNegativeConfirm = "BookFlightFlightNegativeConfirm";
        public const string AmbiguousTimePrompt = "AmbiguousTimePrompt";
    }
}