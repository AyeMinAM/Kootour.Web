//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using SignalR.Hubs;
//using System.Threading.Tasks;

//namespace MVCSite.Biz
//{
//    public class BoardHub : Hub
//    {
//        public void Send()
//        {
//            string userId = Caller.uId;
//            string boardId = Caller.bId;
//            Groups.Add(Context.ConnectionId, boardId);
//            Caller.cId = Context.ConnectionId;
//        }


//        public void SendUpdateList(string boardId,string listId)
//        {
//            Clients[boardId].updateList(listId);
//        }
//        public void SendUpdateListContent(string boardId)
//        {
//            Clients[boardId].updateListContent(boardId);
//        }

//        public void SendUpdateCardAttachments(string boardId,string cardId)
//        {
//            Clients[boardId].updateCardAttachments(cardId);
//        }
//        public void SendMoveCard(string boardId, string sendId,string recvId)
//        {
//            Clients[boardId].moveCard(boardId);
//        }

//        public void SendRefreshBoard(string boardId)
//        {
//            Clients[boardId].refreshBoard(Caller.cId);
//        }


//        public void SendUpdateFavListContent(string favId)
//        {
//            Clients[favId].updateFavListContent(favId);
//        }

//        //public Task Disconnect()
//        //{
//        //    return Clients.leave(Context.ConnectionId, DateTime.Now.ToString());
//        //}

//        //public Task Connect()
//        //{
//        //    string userId = Caller.uId;
//        //    string boardId = Caller.bId;
//        //    return Groups.Add(userId, boardId);
//        //    //return Clients.joined(Context.ConnectionId, DateTime.Now.ToString());
//        //}

//        //public Task Reconnect(IEnumerable<string> groups)
//        //{
//        //    return Clients.rejoined(Context.ConnectionId, DateTime.Now.ToString());
//        //}
//    }


//}