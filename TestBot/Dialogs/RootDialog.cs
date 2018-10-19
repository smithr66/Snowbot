using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;

namespace TestBot.Dialogs
{
	[Serializable]
	public class RootDialog : IDialog<object>
	{
		public enum UserChoices
		{
			CheckTicketStatus,
			OpenTicket,
			OrderItem,
		}
		public async Task StartAsync(IDialogContext context)
		{
			//await context.PostAsync("Hi - I'm Snowbot and I'm here to help");
			context.Wait(this.ShowUserChoices);
		}

		public virtual async Task ShowUserChoices(IDialogContext context, IAwaitable <IMessageActivity> activity) 			
		{
			var message = await activity;
			var choiceOptions = new UserChoices[] { UserChoices.CheckTicketStatus, UserChoices.OpenTicket, UserChoices.OrderItem };
			var choiceDescriptions = new string[] { "Check Ticket Status", "Open Ticket", "Order Equipment" };

			PromptDialog.Choice(
				context: context,
				resume: ChoiceReceivedAsync,
				prompt: "Please select an option",
				retry: "Please select from one of the displayed choices",
				promptStyle: PromptStyle.Auto,
				options: choiceOptions,
				descriptions: choiceDescriptions
				);
		}

		public virtual async Task ChoiceReceivedAsync(IDialogContext context, IAwaitable<UserChoices> activity)
		{
			UserChoices response = await activity;

			var choiceSelected = response.ToString();

			switch (choiceSelected)
			{
				case "CheckTicketStatus":
					context.Call<object>(new CheckTicketStatusDialog(), ChildDialogComplete);
					break;
				case "OpenTicket":
					context.Call<object>(new OpenTicketDialog(), ChildDialogComplete);
					break;
			}

		}

		public virtual async Task ChildDialogComplete(IDialogContext context, IAwaitable<object> response)
		{
			await context.PostAsync("Is there anything else I can help you with today?");
			//context.Wait(this.ShowUserChoices);
			context.Done(this);
		}
	}
}