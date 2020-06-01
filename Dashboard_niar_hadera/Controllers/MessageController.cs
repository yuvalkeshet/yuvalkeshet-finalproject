using Dashboard_niar_hadera.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
//using System.Web.Mvc;

namespace Dashboard_niar_hadera.Controllers
{
    public class MessageController : ApiController
    {
        // GET: Message

        [Route("api/message")]
        public IHttpActionResult Post([FromBody]Message msg)
        {
            return Json(new { msg = msg.sendMessage() });
        }

        [Route("api/message/my_employees")]
        public IHttpActionResult PostToMyEmployees([FromBody]Message msg)
        {
            return Json(new { msg = msg.sendMessageToMyEmployees() });
        }

        [Route("api/message/all")]
        public IHttpActionResult PostToAll([FromBody]Message msg)
        {
            return Json(new { msg = msg.sendMessageToAll() });
        }

        [Route("api/message/department")]
        public IHttpActionResult PostToDepartment([FromBody]Message msg, string depName)
        {
            return Json(new { msg = msg.sendMessageToDep(depName) });
        }

        [Route("api/message/delete")]
        public IHttpActionResult PostToTrash([FromBody]Message msg, bool byReciever=false, bool bySender=false)
        {
            return Json(new { msg = msg.delete(byReciever, bySender) });
        }

        [HttpGet]
        [Route("api/messages")]
        public List<Message> Get(string email, bool receivedOrSent)
        {
            Message msg = new Message();

            return msg.GetMessages(email, receivedOrSent);
        }

        [HttpGet]
        [Route("api/messages/trash")]
        public List<Message> Get(string email)
        {
            Message msg = new Message();

            return msg.GetMessagesFromTrash(email);
        }

        [HttpGet]
        [Route("api/message")]
        public Message Get(string email, int msgCode)
        {
            Message msg = new Message();

            return msg.GetMessage(email, msgCode);
        }

        [HttpPut]
        [Route("api/message")]
        public int PUT_Imp(string email, int msgCode, bool isImp)
        {
            Message msg = new Message();

            return msg.putImp(email, msgCode, isImp);
        }

        [HttpPut]
        [Route("api/message")]
        public int PUT_Read(string email, int msgCode, bool isRead)
        {
            Message msg = new Message();

            return msg.putRead(email, msgCode, isRead);
        }

    }
}
