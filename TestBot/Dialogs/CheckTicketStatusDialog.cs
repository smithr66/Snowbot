using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestBot.Dialogs
{
	[Serializable]
	public class CheckTicketStatusDialog : IDialog<object>
	{

		public async Task StartAsync(IDialogContext context)
		{
			PromptDialog.Text(context, AfterTicketRecievedAsync, "Please enter the ticket number.", attempts: 5);
			//await context.PostAsync("Please enter a ticket number.");
		}
		
		public async Task AfterTicketRecievedAsync(IDialogContext context, IAwaitable<string> userInput)
		{
			var inputText = await userInput;
			await context.PostAsync("Getting information on " + inputText);
			//SN API to retrieve ticket info

			context.Done<object>(new object());

		}
		
	}
}