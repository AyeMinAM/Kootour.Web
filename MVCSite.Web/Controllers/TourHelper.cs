using MVCSite.DAC.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Twilio;
namespace MVCSite.Web.Controllers
{
    public class TourHelper
    {
        private static Random _rdm = new Random((int)DateTime.UtcNow.TimeOfDay.TotalSeconds);
        public static string GetDurationTimeString(int duration, int durationType)
        {
            string result = string.Empty;
            switch ((TourTimeType)durationType)
            {
                case TourTimeType.Days:
                    if (duration == 1)
                        result = "1 Day";
                    else
                        result = string.Format("{0} Days", duration);
                    break;
                case TourTimeType.Hours:
                    if (duration == 1)
                        result = "1 Hour";
                    else
                        result = string.Format("{0} Hours", duration);
                    break;
                case TourTimeType.Minutes:
                    if (duration == 1)
                        result = "1 Minute";
                    else
                        result = string.Format("{0} Minutes", duration);
                    break;
            }
            return result;
        }
        public static TimeSpan GetDurationTimeSpan(int duration, int durationType)
        {
            TimeSpan result;
            switch ((TourTimeType)durationType)
            {
                default:
                case TourTimeType.Days:
                    result = TimeSpan.FromDays(duration);
                    break;
                case TourTimeType.Hours:
                    result = TimeSpan.FromHours(duration);
                    break;
                case TourTimeType.Minutes:
                    result = TimeSpan.FromMinutes(duration);
                    break;
            }
            return result;
        }
        public static void SendSMS(string areaCode, string phone, string message)
        {
            // Find your Account Sid and Auth Token at twilio.com/user/account
            string AccountSid = "AC488541159baf49b68600e3ac36d05331";
            string AuthToken = "ac7548b6787877849e273e1b13125873";
            var twilio = new TwilioRestClient(AccountSid, AuthToken);
            var fullPhone = string.Format("+{0}{1}",areaCode,phone);
            var resultMsg = twilio.SendMessage("+16047574619", fullPhone, message);
           
            //Console.WriteLine(message.Sid);

            ////var smsApi = SinchFactory.CreateApiFactory("fc49856f-c860-4ae1-84f4-546861a430a8", "Tu2tsgFwrkusmmH4LRkqXQ==").CreateSmsApi();
            //var smsApi = SinchFactory.CreateApiFactory("91c42c0b-33a4-42fb-afd4-5936664ee850", "OTOtEU89NEu/e5K8ZWwwcQ==").CreateSmsApi();

            //var sendSmsResponse = await smsApi.Sms(phone, message).Send();
            //await Task.Delay(TimeSpan.FromSeconds(10)); // May take a second or two to be delivered.
            //var smsMessageStatusResponse = await smsApi.GetSmsStatus(sendSmsResponse.MessageId);
        }

        public static string GenerateRandom4DigitString()
        {
            int _min = 1000;
            int _max = 9999;
            return _rdm.Next(_min, _max).ToString();
        }
    }
}