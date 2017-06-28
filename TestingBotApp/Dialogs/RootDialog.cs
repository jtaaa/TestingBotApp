using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Threading;
using System.Text.RegularExpressions;
using System.Linq;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System.Collections.Generic;

namespace TestingBotApp.Dialogs
{
    [LuisModel("05910c2c-3cc3-4cff-b1d0-98e7b0ee630e", "961ddeee452f49a1869de016ac3f0a4d")]
    [Serializable]
    public class RootDialog : LuisDialog<object>
    {
        private readonly Dictionary<string, string> Aliases = new Dictionary<string, string>()
        {
            { "JOSHUA", "Canadan" },
            { "SEENATH", "Hydrodan" },
            { "IRWIN", "Irmyster" }
        };

        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I'm sorry. I didn't understand you.");
            context.Wait(MessageReceived);
        }
        private static string ToUpperFirstLetter(string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;
            // convert to char array of the string
            char[] letters = source.ToCharArray();
            // upper case the first char
            letters[0] = char.ToUpper(letters[0]);
            // return the array made of the new char array
            return new string(letters);
        }

        [LuisIntent("GetAlias")]
        public async Task GetAlias(IDialogContext context, LuisResult result)
        {
            EntityRecommendation name;
            if (result.TryFindEntity("Name", out name))
            {
                string alias = String.Empty;
                if (Aliases.ContainsKey(name.Entity.ToUpper()))
                {
                    alias = Aliases[name.Entity.ToUpper()];
                    await context.PostAsync($"{ToUpperFirstLetter(name.Entity)}'s alias is {alias}");
                }
                else
                {
                    await context.PostAsync($"Sorry I don't know an alias for {ToUpperFirstLetter(name.Entity)}");
                }
            }
            else
            {
                await context.PostAsync("Sorry I didn't catch the person's name");
            }
            context.Wait(MessageReceived);
        }

        [LuisIntent("Greetings")]
        public async Task Greetings(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Hello there!");
            context.Wait(MessageReceived);
        }
    }
}