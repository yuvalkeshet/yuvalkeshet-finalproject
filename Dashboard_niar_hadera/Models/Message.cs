using Dashboard_niar_hadera.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboard_niar_hadera.Models
{
    public class Message
    {
        public int MessageCode { get; set; }
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public DateTime MessageDate { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string[] Files { get; set; }
        public bool IsRead { get; set; }
        public bool IsImportant { get; set; }

        public Message(int _msgCode, string _from, string _to, DateTime _date,
            string _title, string _content, string[] _files, bool _isRead, bool _isImportant)
        {

            FromUser = _from;
            ToUser = _to;
            MessageDate = _date;
            Title = _title;
            Content = _content;
            Files = _files;
            MessageCode = _msgCode;
            IsRead = _isRead;
            IsImportant = _isImportant;
        }

        public Message(string _from, string _to,
           string _title, string _content, string[] _files = null)
        {

            FromUser = _from;
            ToUser = _to;
            Title = _title;
            Content = _content;
            Files = _files;
        }

        public Message(string _from, 
           string _title, string _content, string[] _files = null)
        {

            FromUser = _from;
            Title = _title;
            Content = _content;
            Files = _files;
        }

        public Message(int _msgCode, string _from, string _to, bool _isRead, bool _isImportant)
        {
            MessageCode = _msgCode;
            FromUser = _from;
            ToUser = _to;
            IsRead = _isRead;
            IsImportant = _isImportant;
        }

        public Message(int _msgCode, string _from, string _to)
        {
            MessageCode = _msgCode;
            FromUser = _from;
            ToUser = _to;
           
        }



        public Message()
        {

        }

        public Message sendMessage()
        {
            DBService dbs = new DBService();
            return dbs.sendMessage(this);
        }

        public Message sendMessageToMyEmployees()
        {
            DBService dbs = new DBService();
            return dbs.sendMessageToMyEmployees(this);
        }

        public Message sendMessageToAll()
        {
            DBService dbs = new DBService();
            return dbs.sendMessageToAll(this);
        }

        public Message sendMessageToDep(string depName)
        {
            DBService dbs = new DBService();
            return dbs.sendMessageToDep(this, depName);
        }

        public List<Message> GetMessages(string email, bool receivedOrSent)
        {
            DBService dbs = new DBService();
            return dbs.getMessages(email, receivedOrSent);
        }

        public List<Message> GetMessagesFromTrash(string email)
        {
            DBService dbs = new DBService();
            return dbs.getMessagesFromTrash(email);
        }

        public Message GetMessage(string email, int msgCode)
        {
            DBService dbs = new DBService();
            return dbs.getMessage(email, msgCode);
        }

        public int putImp(string email, int msgCode, bool isImp)
        {
            DBService dbs = new DBService();
            return dbs.setImp(email, msgCode, isImp);
        }

        public int putRead(string email, int msgCode, bool isRead)
        {
            DBService dbs = new DBService();
            return dbs.setRead(email, msgCode, isRead);
        }

        public Message delete(bool byReciever, bool bySender)
        {
            DBService dbs = new DBService();
            return dbs.deleteMessage(this, byReciever, bySender);
        }
    }
}