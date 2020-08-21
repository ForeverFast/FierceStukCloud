using System;
using System.Collections.Generic;
using System.Text;

namespace FierceStukCloud.Core.Other
{
    public class Music_AuthorAndTitleCheck
    {
        public string Author { get; set; }

        public string Title { get; set; }

        
        private void GetTitleAndAuthor_FirstCheck(string value, TagLib.File File_TAG)
        {
            try
            {
                Author = File_TAG.Tag.FirstPerformer;
                Title = File_TAG.Tag.Title;

                if (Author == "" || Title == "" || Author == null || Title == null)
                    GetTitleAndAuthor_SecondCheck(value);
            }
            catch (Exception)
            {
                GetTitleAndAuthor_SecondCheck(value);
            }
        }

        private void GetTitleAndAuthor_SecondCheck(string value)
        {
            try
            {
                Author = value.Substring(0, value.IndexOf(" - "));
                Title = value.Remove(0, value.IndexOf(" - ") + 3);
                Title = Title.Substring(0, Title.LastIndexOf(".mp3"));
                if (Author == "" || Title == "" || Author == null || Title == null)
                    GetTitleAndAuthor_ThirdCheck(value);
            }
            catch (Exception)
            {
                GetTitleAndAuthor_ThirdCheck(value);
            }
        }

        private void GetTitleAndAuthor_ThirdCheck(string value)
        {
            try
            {
                int tempPos = value.IndexOf(" - ");

                Author = value.Substring(0, value.IndexOf(" - "));
                Title = value.Remove(0, value.IndexOf(" - ") + 3);
                Title = Title.Substring(0, Title.LastIndexOf(".mp3"));
                //if (Author == "" || Title == "")
                //    GetTitleAndAuthor_ThirdCheck(value);
            }
            catch (Exception)
            {
                //GetTitleAndAuthor_ThirdCheck(value);
            }
        }

        public Music_AuthorAndTitleCheck(string value, TagLib.File File_TAG) => GetTitleAndAuthor_FirstCheck(value, File_TAG);
       
    }
}
