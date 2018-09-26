using System;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using AdaptiveCards;
using System.Collections.Generic;

namespace GoForwardBot.Dialogs
{
    [Serializable]
    public class DeadlineDialog_t : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            //await context.PostAsync($"What is the deadline of your task?\n\n(e.g. '28 / 04 / 2019')");
            //ShowOptionsAsync(context);
            context.Wait(this.MessageReceivedStart);
        }

        private async Task MessageReceivedStart(IDialogContext context, IAwaitable<IMessageActivity> result)
        {

            var activity = await result;

            if (activity != null && activity.Text != null)
            {
                var replyMessage = context.MakeMessage();
                //replyMessage.Attachments = new List<Attachment>();
                ShowAdaptiveCard(replyMessage);
                
                await context.PostAsync(replyMessage);

            }

            context.Wait(MessageReceivedStart);
        }

        private static void ShowSignInCard(IMessageActivity replyMessage)
        {
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction plButton = new CardAction()
            {
                Value = "https://<OAuthSignInURL>",
                Type = "signin",
                Title = "Connect"
            };
            cardButtons.Add(plButton);
            SigninCard plCard = new SigninCard(text: "You need to authorize me", buttons: cardButtons);
            Attachment plAttachment = plCard.ToAttachment();
            replyMessage.Attachments.Add(plAttachment);
        }

        private static void ShowReceiptCard(IMessageActivity replyMessage)
        {
            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage(url: "http://lorempixel.com/200/200/food"));
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction plButton = new CardAction()
            {
                Value = "https://en.wikipedia.org/wiki/Pig_Latin",
                Type = "openUrl",
                Title = "Wikipedia Page"
            };
            cardButtons.Add(plButton);
            ReceiptItem lineItem1 = new ReceiptItem()
            {
                Title = "Pork Shoulder",
                Subtitle = "8 lbs",
                Text = null,
                Image = new CardImage(url: "http://lorempixel.com/200/200/food"),
                Price = "16.25",
                Quantity = "1",
                Tap = null
            };
            ReceiptItem lineItem2 = new ReceiptItem()
            {
                Title = "Bacon",
                Subtitle = "5 lbs",
                Text = null,
                Image = new CardImage(url: "http://lorempixel.com/200/200/food"),
                Price = "34.50",
                Quantity = "2",
                Tap = null
            };
            List<ReceiptItem> receiptList = new List<ReceiptItem>();
            receiptList.Add(lineItem1);
            receiptList.Add(lineItem2);
            ReceiptCard plCard = new ReceiptCard()
            {
                Title = "I'm a receipt card, isn't this bacon expensive?",
                Buttons = cardButtons,
                Items = receiptList,
                Total = "275.25",
                Tax = "27.52"
            };
            Attachment plAttachment = plCard.ToAttachment();
            replyMessage.Attachments.Add(plAttachment);
        }

        private static void ShowThumbnailCard(IMessageActivity replyMessage)
        {
            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage(url: "http://lorempixel.com/200/200/food"));
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction plButton = new CardAction()
            {
                Value = "https://en.wikipedia.org/wiki/Pig_Latin",
                Type = "openUrl",
                Title = "Wikipedia Page"
            };
            cardButtons.Add(plButton);
            ThumbnailCard plCard = new ThumbnailCard()
            {
                Title = "I'm a thumbnail card",
                Subtitle = "Wikipedia Page",
                Images = cardImages,
                Buttons = cardButtons
            };
            Attachment plAttachment = plCard.ToAttachment();
            replyMessage.Attachments.Add(plAttachment);
        }

        private static void ShowHeroCard(IMessageActivity replyMessage)
        {
            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage(url: "http://lorempixel.com/200/200/food"));
            cardImages.Add(new CardImage(url: "http://lorempixel.com/200/200/food"));
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction plButton = new CardAction()
            {
                Value = "https://en.wikipedia.org/wiki/Pig_Latin",
                Type = "openUrl",
                Title = "Wikipedia Page"
            };
            cardButtons.Add(plButton);
            HeroCard plCard = new HeroCard()
            {
                Title = "I'm a hero card",
                Subtitle = "Wikipedia Page",
                Images = cardImages,
                Buttons = cardButtons
            };
            Attachment plAttachment = plCard.ToAttachment();
            replyMessage.Attachments.Add(plAttachment);
        }

        private static void ShowStaticCard(IMessageActivity replyMessage)
        {
            replyMessage.Attachments.Add(new Attachment()
            {
                ContentUrl = "http://lorempixel.com/200/200/food",
                ContentType = "image/png",
                Name = "food.png"
            });
        }

        private static void ShowAdaptiveCard(IMessageActivity replyMessage)
        {
            AdaptiveCard card = new AdaptiveCard();

            // Add text to the card.
            card.Body.Add(new TextBlock()
            {
                Text = "Adaptive Card design session",
                Size = TextSize.Large,
                Weight = TextWeight.Bolder
            });
            
            card.Body.Add(new DateInput()
            {
                Id = "Checkin",
                Speak = "<s>When do you want to check in?</s>"
            });
            
            // Add buttons to the card.
            card.Actions.Add(new HttpAction()
            {
                Url = "http://foo.com",
                Title = "Snooze"
            });

            card.Actions.Add(new HttpAction()
            {
                Url = "http://foo.com",
                Title = "I'll be late"
            });

            card.Actions.Add(new HttpAction()
            {
                Url = "http://foo.com",
                Title = "Dismiss"
            });

            
            // Create the attachment.
            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };
            //Attachment plAttachment = card.ToAttachment();
            replyMessage.Attachments.Add(attachment);
        }
    }
}