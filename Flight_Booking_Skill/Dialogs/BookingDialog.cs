using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AdaptiveCards;
using Flight_Booking_Skill.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Builder.Solutions.Extensions;
using Microsoft.Bot.Builder.Solutions.Responses;
using Flight_Booking_Skill.Utilities;
using Flight_Booking_Skill.Responses.FlightBooking;
using Microsoft.Azure.Documents.Client;
using Microsoft.Bot.Schema;
using Flight_Booking_Skill.Models;

namespace Flight_Booking_Skill.Dialogs
{
    public class BookingDialog : SkillDialogBase
    {
        List<string> flights = new List<String>() {
               "EGYPT","FRANCE","ENGLAND"
           };
        static Reservation _reservation = new Reservation();
        public BookingDialog(
            BotSettings settings,
            BotServices services,
            ResponseManager responseManager,
            ConversationState conversationState,
            IBotTelemetryClient telemetryClient)
           : base(nameof(BookingDialog), settings, services, responseManager, conversationState, telemetryClient)
        {

            // Restaurant Booking waterfall
            var bookingWaterfall = new WaterfallStep[]
            {
                Init,
                AskForDestination,
                AskForReservationDate,
                AskForNumberOfTickets,
                AskForReservationConfirmation,
                End
            };

            AddDialog(new WaterfallDialog(Actions.BookFlight, bookingWaterfall));

            // Prompts
            AddDialog(new ChoicePrompt(Actions.AskForDestination) { Style = ListStyle.Inline, ChoiceOptions = new ChoiceFactoryOptions { IncludeNumbers = true } });
            AddDialog(new DateTimePrompt(Actions.AskReservationDateStep));
            AddDialog(new NumberPrompt<int>(Actions.AskAttendeeCountStep));
            AddDialog(new ConfirmPrompt(Actions.ConfirmSelectionBeforeBookingStep));
            AddDialog(new ConfirmPrompt("Make Sure"));

            // Optional
            AddDialog(new ChoicePrompt(Actions.AmbiguousTimePrompt) { Style = ListStyle.HeroCard, ChoiceOptions = new ChoiceFactoryOptions { IncludeNumbers = true } });

            // Set starting dialog for component
            InitialDialogId = Actions.BookFlight;


        }

        

        private async Task<DialogTurnResult> Init(WaterfallStepContext sc, CancellationToken cancellationToken = default(CancellationToken))
        {

            // This would be passed from the Virtual Assistant moving forward
            var tokens = new StringDictionary
            {
                { "UserName", null ?? "UserName" }
            };


            // Start the flow

            var reply = ResponseManager.GetResponse(FlightBookingSharedResponses.BookFlightFlowStartMessage,tokens);
            await sc.Context.SendActivityAsync(reply);

            return await sc.NextAsync(sc.Values, cancellationToken);
        }

        private async Task<DialogTurnResult> AskForDestination(WaterfallStepContext sc, CancellationToken cancellationToken)
        {

            //use http client to get data from api
        
            var options = new PromptOptions()
            {
                Choices = new List<Choice>(),
            };

            foreach (var flight in flights)
            {
                options.Choices.Add(new Choice(flight));
            }
            var tokens = new StringDictionary
            {
                { "UserName", null ?? "Mahmoud" }
            };
            var replyMessage = ResponseManager.GetResponse(

               FlightBookingSharedResponses.BookFlightDestinationSelectionPrompt
               );

            // Prompt for flight choice
            return  await sc.PromptAsync(Actions.AskForDestination, new PromptOptions { Prompt = replyMessage, Choices = options.Choices }, cancellationToken);

        }

        private async Task<DialogTurnResult> AskForReservationDate(WaterfallStepContext sc, CancellationToken cancellationToken)
        {
            _reservation.Destination = flights[int.Parse(sc.Context.Activity.Text)-1] ;

            #region TRY



            //    cards.Add(new Card("ChooseDate");

            //Card ReservationDate = new Card("ChooseDate");
            //var replyMessage = ResponseManager.GetCardResponse(ReservationDate);

            var replyMessage = ResponseManager.GetResponse(
           FlightBookingSharedResponses.BookFlightDatePrompt
           );


            #endregion


            //return await sc.Context.SendActivityAsync(replyMessage);
            // Prompt for DateTime 
            return await sc.PromptAsync(Actions.AskReservationDateStep, new PromptOptions { Prompt = replyMessage }, cancellationToken);
        }
        private async Task<DialogTurnResult> AskForNumberOfTickets(WaterfallStepContext sc, CancellationToken cancellationToken)
        {
            _reservation.ReservationDate = sc.Context.Activity.Text;
            var replyMessage = ResponseManager.GetResponse(
               FlightBookingSharedResponses.BookFlightAttendeePrompt
               );

            
            // Prompt for Attendee Count 
            return await sc.PromptAsync(Actions.AskAttendeeCountStep, new PromptOptions { Prompt = replyMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> AskForReservationConfirmation(WaterfallStepContext sc, CancellationToken cancellationToken)
        {
            _reservation.NumberOfTickets = int.Parse(sc.Context.Activity.Text);

            var tokens = new StringDictionary
            {
                { "AttendeeCount", _reservation.NumberOfTickets.ToString() },
                { "Destination",_reservation.Destination},
                {"ReservationTime",_reservation.ReservationDate }
            };

            var replyMessage = ResponseManager.GetResponse(
          FlightBookingSharedResponses.BookFlightConfirmationPrompt,tokens
          );
            // Prompt for Confirm 
            return await sc.PromptAsync(Actions.ConfirmSelectionBeforeBookingStep, new PromptOptions { Prompt = replyMessage }, cancellationToken);
        }


        private async Task<DialogTurnResult> End(WaterfallStepContext sc, CancellationToken cancellationToken)
        {
            var result = sc.Context.Activity.Text;
            if (result == "Yes")
            {
                var tokens = new StringDictionary
            {
                { "AttendeeCount", _reservation.NumberOfTickets.ToString() },
                { "Destination",_reservation.Destination},
                {"ReservationTime",_reservation.ReservationDate },
                { "Flight","Flight Number 29409"}
            };
                var replyMessage = ResponseManager.GetResponse(
              FlightBookingSharedResponses.BookFlightAcceptedMessage, tokens
              );
                _reservation = new Reservation();

                return await sc.PromptAsync("Make Sure", new PromptOptions { Prompt = replyMessage }, cancellationToken);

            }
            else { }
            // Prompt for Confirm 
            _reservation = new Reservation();
            return await sc.EndDialogAsync(cancellationToken: cancellationToken);
        }
    }
}
