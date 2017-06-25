using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Threading;
using System.Text.RegularExpressions;
using System.Linq;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

namespace TestingBotApp.Dialogs
{
    [LuisModel("05910c2c-3cc3-4cff-b1d0-98e7b0ee630e", "961ddeee452f49a1869de016ac3f0a4d")]
    [Serializable]
    public class RootDialog : LuisDialog<object>
    {

        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Sorry I did not understand: " + string.Join(", ", result.Intents.Select(i => i.Intent));
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("GetAlias")]
        public async Task GetAlias(IDialogContext context, LuisResult result)
        {
            EntityRecommendation title;
            string reply = "";
            if (result.TryFindEntity("Name", out title))
            {
                reply = title.Entity;
            }

            await context.PostAsync(reply);
            context.Wait(MessageReceived);
        }
    }
}