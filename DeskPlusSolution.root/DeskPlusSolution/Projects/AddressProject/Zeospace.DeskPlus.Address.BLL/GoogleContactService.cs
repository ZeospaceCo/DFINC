using System;
using System.Collections.Generic;
using Google.Contacts;
using Google.GData.Client;
using Google.GData.Contacts;
using Zeospace.DeskPlus.Address.BE;

namespace Zeospace.DeskPlus.Address.BLL
{
    public class GoogleContactService
    {
        public ContactsService GContactService = null;
        private string UserName = "";
        private string UserPwd = "";

        /// <summary>
        /// 심재운 2014.11.21 수정
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public GoogleContactService(string username,
            string password)
        {
            //GContactService = new ContactsService("Contact Infomation");
            //GContactService.setUserCredentials(username, password);
            UserName = username;
            UserPwd = password;
        }

        /// <summary>
        /// 작성자 : 심재운 2014.11.21 수정
        /// 구글 주소록 전체 가져오기
        /// </summary>
        /// <param name="maxCount">The max count.</param>
        /// <returns></returns>
        public List<GoogleContactInfo> GetContact(int maxCount, DateTime _startDate)
        {
            List<GoogleContactInfo> contactDetails = new List<GoogleContactInfo>();
            if (UserName.Length == 0 || UserPwd.Length == 0) return contactDetails;

            RequestSettings req = new RequestSettings("", UserName, UserPwd);
            req.AutoPaging = true;

            // Fetch contacts
            ContactsRequest cRequest = new ContactsRequest(req);
            Feed<Contact> fContacts = cRequest.GetContacts();
            //Lisrt of Contacts
            foreach (Contact entry in fContacts.Entries)
            {
                BE.GoogleContactInfo contact = new BE.GoogleContactInfo
                {
                    FullName = entry.Title,
                    OfficeEmail = entry.Emails.Count >= 1 ? entry.Emails[0].Address : "",
                    HomeEmail = entry.Emails.Count >= 2 ? entry.Emails[1].Address : "",
                    MPhone = entry.Phonenumbers.Count >= 1 ? entry.Phonenumbers[0].Value : "",
                    CPhone = entry.Phonenumbers.Count >= 2 ? entry.Phonenumbers[1].Value : "",
                    HPhone = entry.Phonenumbers.Count >= 3 ? entry.Phonenumbers[2].Value : "",
                    //SelfUrl = entry.SelfUri.Content ?? "",
                    OfficeAddress = entry.PostalAddresses.Count >= 1 ? entry.PostalAddresses[0].FormattedAddress : "",
                    HomeAddress = entry.PostalAddresses.Count >= 2 ? entry.PostalAddresses[1].FormattedAddress : "",
                    //Details = entry.Content.Content,
                    CompanyName = entry.Organizations.Count >= 1 ? entry.Organizations[0].Name : "",
                    UpDated = entry.Updated
                };

                contactDetails.Add(contact);
            }

            //List<GoogleContactInfo> contactDetails = new List<GoogleContactInfo>();
            //ContactsQuery query = new ContactsQuery(ContactsQuery.
            //    CreateContactsUri("default"));

            ///*
            // *  NumberToRetrieve
            //        -Set the maximum number of entries to return.
            //    StartIndex
            //        -Sets the 1-based index of the first result to be retrieved (for paging through results).
            //    StartDate
            //        -Set the lower bound on entry update dates.
            //    ShowDeleted
            //        -Include deleted contacts in the returned contacts feed. Deleted contacts are shown as entries that contain nothing but an <atom:id> element and a <gd:deleted> element. (Google retains placeholders for deleted contacts for 30 days after deletion; during that time, you can request the placeholders using the showdeleted query parameter.)
            //    OrderBy
            //        -You can use the value "lastmodified" to have the contacts returned by last-modified date.
            //    SortOrder
            //        -Set sorting order direction. This can be either "ascending" or "descending".
            // * */

            //query.NumberToRetrieve = maxCount;
            //query.OrderBy = "lastmodified";
            //query.SortOrder = "ascending";
            //query.StartDate = _startDate;// new DateTime(2010, 1, 1);

            //ContactsFeed feed = GContactService.Query(query);
            //foreach (ContactEntry entry in feed.Entries)
            //{
            //    BE.GoogleContactInfo contact = new BE.GoogleContactInfo
            //    {
            //        FullName = entry.Title.Text,
            //        OfficeEmail = entry.Emails.Count >= 1 ? entry.Emails[0].Address : "",
            //        HomeEmail = entry.Emails.Count >= 2 ? entry.Emails[1].Address : "",
            //        MPhone = entry.Phonenumbers.Count >= 1 ? entry.Phonenumbers[0].Value : "",
            //        CPhone = entry.Phonenumbers.Count >= 2 ? entry.Phonenumbers[1].Value : "",
            //        HPhone = entry.Phonenumbers.Count >= 3 ? entry.Phonenumbers[2].Value : "",
            //        SelfUrl = entry.SelfUri.Content ?? "",
            //        OfficeAddress = entry.PostalAddresses.Count >= 1 ? entry.PostalAddresses[0].FormattedAddress : "",
            //        HomeAddress = entry.PostalAddresses.Count >= 2 ? entry.PostalAddresses[1].FormattedAddress : "",
            //        Details = entry.Content.Content,
            //        CompanyName   = entry.Organizations.Count >= 1 ? entry.Organizations[0].Name : "",
            //        UpDated = entry.Updated
            //    };

            //    contactDetails.Add(contact);
            //}

            return contactDetails;
        }
    }
}