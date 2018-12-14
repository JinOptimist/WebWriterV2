using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebWriterV2.FrontModel.Email
{
    public class QuestionAnswerPairEmail
    {
        public string QuestionText { get; set; }
        public List<string> AnswersText { get; set; }
        public string OtherAnswerText { get; set; }
    }
}