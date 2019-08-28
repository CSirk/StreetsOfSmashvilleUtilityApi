using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Speech.Synthesis;
using System.Web.Http;
using Wavify.Core.Actions;

namespace StreetsOfSmashvilleUtilityApi.Api.Controllers
{
    public class AudioController : ApiController
    {
        [HttpPost]
        [Route("getMp3BytesForSpeechPromptCollection")]
        public HttpResponseMessage GetMp3BytesForSpeechPromptCollection([FromBody] SpeechPromptCollection speechPromptCollection)
        {
            var promptBuilder = new PromptBuilder();

            foreach (var speakPause in speechPromptCollection.SpeakPauseCollection)
            {
                var speakSection = speakPause.Key;
                var pauseTime = speakPause.Value;

                promptBuilder.AppendText(speakSection);
                AppendBreakToPrompt(promptBuilder, pauseTime);
            }

            var mp3ByteArray = SpeechAction.ConvertSpeechSynthPromptToMp3ByteArray(promptBuilder);

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(mp3ByteArray)
            };

            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = $"Workout_Assistant_{DateTime.Now}_Workout.mp3"
            };

            //result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            return result;
        }


        private PromptBuilder AppendBreakToPrompt(PromptBuilder prompt, int seconds)
        {
            prompt.AppendBreak(new TimeSpan(0, 0, seconds));
            return prompt;
        }
    }


    public class SpeechPromptCollection
    {
        public Dictionary<string, int> SpeakPauseCollection { get; set; }
    }
}
