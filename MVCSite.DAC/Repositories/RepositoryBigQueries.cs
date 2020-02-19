using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVCSite.DAC.Entities;
using System.Data.Objects;
using System.Data;
using MVCSite.DAC.Common;
using System.Data.SqlClient;
using System.Data.Common;
using MVCSite.Common;

namespace MVCSite.DAC.Repositories
{
    public class RepositoryBigQueries
    {
        private const string SPNAME_tourGetAllSimpleInfoByIDs = "tourGetAllSimpleInfoByIDs";

        #region public static void tourGetAllSimpleInfoByIDs
        public static void tourGetAllSimpleInfoByIDs(string tourIds, 
            out List<Tour> tourList,
            out List<TourPicture> tourPictureList,
            out List<TourSchedule> tourScheduleList,
            out List<TourVendorPromo> tourVendorPromoList,
            out List<User> userList
            )
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter("@TourIds", SqlDbType.VarChar)
			};

            parms[0].Value = tourIds;
            tourList = new List<Tour>();
            tourPictureList = new List<TourPicture>();
            tourScheduleList = new List<TourSchedule>();
            tourVendorPromoList = new List<TourVendorPromo>();
            userList = new List<User>();

            using (DbDataReader reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING_KOOTOUR, CommandType.StoredProcedure, SPNAME_tourGetAllSimpleInfoByIDs, parms))
            {
                while (reader.Read())
                {
                    tourList.Add(new Tour(reader));
                }

                reader.NextResult();
                while (reader.Read())
                {
                    tourPictureList.Add(new TourPicture(reader));
                }

                reader.NextResult();
                while (reader.Read())
                {
                    tourScheduleList.Add(new TourSchedule(reader));
                }

                reader.NextResult();
                while (reader.Read())
                {
                    tourVendorPromoList.Add(new TourVendorPromo(reader));
                }

                reader.NextResult();
                while (reader.Read())
                {
                    userList.Add(new User(reader));
                }
            }
        }
        #endregion


        private const string SPNAME_tourGetAllTouristInfoByUserID = "tourGetAllTouristInfoByUserID";

        #region public static void TourGetAllTouristInfoByUserID
        public static void TourGetAllTouristInfoByUserID(int userID, out List<UserWish> wishList,out List<UserTourBooking> userTourBookingList,
            out List<Tour> tourList,
            out List<TourPicture> tourPictureList,
            out List<TourSchedule> tourScheduleList,
            out List<TourVendorPromo> tourVendorPromoList,
            out List<TourExtra> extraList,
            out List<UserTourReview> userTourReviewList
            )
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter("@UserID", SqlDbType.Int)
			};

            parms[0].Value = userID;
            wishList = new List<UserWish>();
            userTourBookingList = new List<UserTourBooking>();
            tourList = new List<Tour>();
            tourPictureList = new List<TourPicture>();
            tourScheduleList = new List<TourSchedule>();
            tourVendorPromoList = new List<TourVendorPromo>();
            extraList = new List<TourExtra>();
            userTourReviewList = new List<UserTourReview>();
            using (DbDataReader reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING_KOOTOUR, CommandType.StoredProcedure, SPNAME_tourGetAllTouristInfoByUserID, parms))
            {
                while (reader.Read())
                {
                    wishList.Add(new UserWish(reader));
                }

                reader.NextResult();
                while (reader.Read())
                {
                    userTourBookingList.Add(new UserTourBooking(reader));
                }

                reader.NextResult();
                while (reader.Read())
                {
                    tourList.Add(new Tour(reader));
                }

                reader.NextResult();
                while (reader.Read())
                {
                    tourPictureList.Add(new TourPicture(reader));
                }

                reader.NextResult();
                while (reader.Read())
                {
                    tourScheduleList.Add(new TourSchedule(reader));
                }

                reader.NextResult();
                while (reader.Read())
                {
                    tourVendorPromoList.Add(new TourVendorPromo(reader));
                }

                reader.NextResult();
                while (reader.Read())
                {
                    extraList.Add(new TourExtra(reader));
                }

                reader.NextResult();
                while (reader.Read())
                {
                    userTourReviewList.Add(new UserTourReview(reader));
                }
            }
        }

        #endregion


        private const string SPNAME_tourGetAllSimpleInfoByUserID = "tourGetAllSimpleInfoByUserID";

        #region public static void TourGetAllSimpleInfoByUserID
        public static void TourGetAllSimpleInfoByUserID(int userID, out List<Tour> tourList,
            out List<TourPicture> tourPictureList,
            out List<TourSchedule> tourScheduleList,
            out List<TourVendorPromo> tourVendorPromoList,
            out List<UserTourReview> userTourReviewList,
            out List<User> userList
            )
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter("@UserID", SqlDbType.Int)
			};

            parms[0].Value = userID;
            tourList = new List<Tour>();
            tourPictureList = new List<TourPicture>();
            tourScheduleList = new List<TourSchedule>();
            tourVendorPromoList = new List<TourVendorPromo>();
            userTourReviewList = new List<UserTourReview>();
            userList=new List<User>();
            using (DbDataReader reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING_KOOTOUR, CommandType.StoredProcedure, SPNAME_tourGetAllSimpleInfoByUserID, parms))
            {
                while (reader.Read())
                {
                    tourList.Add(new Tour(reader));
                }
                reader.NextResult();
                while (reader.Read())
                {
                    tourPictureList.Add(new TourPicture(reader));
                }
                reader.NextResult();
                while (reader.Read())
                {
                    tourScheduleList.Add(new TourSchedule(reader));
                }
                reader.NextResult();
                while (reader.Read())
                {
                    tourVendorPromoList.Add(new TourVendorPromo(reader));
                }
                reader.NextResult();
                while (reader.Read())
                {
                    userTourReviewList.Add(new UserTourReview(reader));
                }
                reader.NextResult();
                while (reader.Read())
                {
                    userList.Add(new User(reader));
                }
            }
        }

        #endregion


        private const string SPNAME_tourGetAllInfoByID = "tourGetAllInfoByID";

        #region public static void TourGetAllInfoByID(int tourID, out TourObject tourObject, out TourExclusionList tourExclusionList, out TourInclusionList tourInclusionList, out TourPictureList tourPictureList, out TourScheduleObject tourScheduleObject,out UserTourReviewObject userTourReviewObject)

        /// <summary>
        /// Summary of method TourGetAllInfoByID.
        /// </summary>
        /// <param name="tourID">TourID</param>
        /// <param name="tourObject">Out TourObject</param>
        /// <param name="tourExclusionList">Out TourExclusionList</param>
        /// <param name="tourInclusionList">Out TourInclusionList</param>
        /// <param name="tourPictureList">Out TourPictureList</param>
        /// <param name="tourScheduleObject">Out TourScheduleObject</param>
        public static void TourGetAllInfoByID(
            int tourID, string calendar, 
            out Tour tourObject, out List<TourExclusion> tourExclusionList, out List<TourInclusion> tourInclusionList, 
            out List<TourPicture> tourPictureList, out List<TourExtra> tourExtraList, out List<TourPriceBreakdown> tourPriceBreakdownList, 
            out TourSchedule tourScheduleObject, out List<TourVendorPromo> tourVendorPromoList, out List<UserTourBooking> userTourBookingList,
            out List<User> userList       
            )
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter("@TourID", SqlDbType.Int),
				new SqlParameter("@Calendar", SqlDbType.VarChar)
			};

            parms[0].Value = tourID;
            parms[1].Value = calendar;

            tourObject = null;
            tourExclusionList = new List<TourExclusion>();
            tourInclusionList = new List<TourInclusion>();
            tourPictureList = new List<TourPicture>();
            tourScheduleObject = null;
            tourVendorPromoList = new List<TourVendorPromo>(); ;
            tourExtraList = new List<TourExtra>();
            tourPriceBreakdownList = new List<TourPriceBreakdown>();
            userTourBookingList = new List<UserTourBooking>();
            userList = new List<User>();
            using (DbDataReader reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING_KOOTOUR, CommandType.StoredProcedure, SPNAME_tourGetAllInfoByID, parms))
            {
                if (reader.Read())
                    tourObject = new Tour(reader);

                reader.NextResult();
                while (reader.Read())
                {
                    tourExclusionList.Add(new TourExclusion(reader));
                }

                reader.NextResult();
                while (reader.Read())
                {
                    tourInclusionList.Add(new TourInclusion(reader));
                }

                reader.NextResult();
                while (reader.Read())
                {
                    tourPictureList.Add(new TourPicture(reader));
                }

                reader.NextResult();
                while (reader.Read())
                {
                    tourExtraList.Add(new TourExtra(reader));
                }

                reader.NextResult();
                while (reader.Read())
                {
                    tourPriceBreakdownList.Add(new TourPriceBreakdown(reader));
                }

                reader.NextResult();
                if (reader.Read())
                    tourScheduleObject = new TourSchedule(reader);

                reader.NextResult();
                while (reader.Read())
                {
                    tourVendorPromoList.Add(new TourVendorPromo(reader));
                }

                reader.NextResult();
                while (reader.Read())
                {
                    userTourBookingList.Add(new UserTourBooking(reader));
                }

                reader.NextResult();
                while (reader.Read())
                {
                    userList.Add(new User(reader));
                }
            }
        }

        #endregion

        private const string SPNAME_tourGetAllInfoByUserIDTourIDCalendar = "tourGetAllInfoByUserIDTourIDCalendar";
        #region public static void TourGetAllInfoByUserIDTourIDCalendar(int userID, int tourID, out TourObject tourObject, out TourExclusionList tourExclusionList, out TourInclusionList tourInclusionList, out TourPictureList tourPictureList, out TourScheduleObject tourScheduleObject,out UserTourReviewObject userTourReviewObject)
        public static void TourGetAllInfoByUserIDTourIDCalendar(
            int userID, int tourID, string calendar, 
            out Tour tourObject, out List<TourExclusion> tourExclusionList, out List<TourInclusion> tourInclusionList, 
            out List<TourPicture> tourPictureList, out List<TourExtra> tourExtraList, out List<TourPriceBreakdown> tourPriceBreakdownList, 
            out TourSchedule tourScheduleObject, out List<TourVendorPromo> tourVendorPromoList, out List<GuiderExcludedDates> guiderExcludedDatesList, 
            out List<UserTourBooking> userTourBookingList, out List<User> userList
            )
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@UserID", SqlDbType.Int),
                new SqlParameter("@TourID", SqlDbType.Int),
                new SqlParameter("@Calendar", SqlDbType.VarChar)
            };
            parms[0].Value = userID;
            parms[1].Value = tourID;
            parms[2].Value = calendar;

            tourObject = null;
            tourExclusionList = new List<TourExclusion>();
            tourInclusionList = new List<TourInclusion>();
            tourPictureList = new List<TourPicture>();
            tourScheduleObject = null;
            tourVendorPromoList = new List<TourVendorPromo>();
            guiderExcludedDatesList = new List<GuiderExcludedDates>();

            tourExtraList = new List<TourExtra>();
            tourPriceBreakdownList = new List<TourPriceBreakdown>();
            userTourBookingList = new List<UserTourBooking>();
            userList = new List<User>();
            using (DbDataReader reader = SqlHelper.ExecuteReader(SqlHelper.CONN_STRING_KOOTOUR, CommandType.StoredProcedure, SPNAME_tourGetAllInfoByUserIDTourIDCalendar, parms))
            {
                if (reader.Read())
                    tourObject = new Tour(reader);

                reader.NextResult();
                while (reader.Read())
                {
                    tourExclusionList.Add(new TourExclusion(reader));
                }

                reader.NextResult();
                while (reader.Read())
                {
                    tourInclusionList.Add(new TourInclusion(reader));
                }

                reader.NextResult();
                while (reader.Read())
                {
                    tourPictureList.Add(new TourPicture(reader));
                }

                reader.NextResult();
                while (reader.Read())
                {
                    tourExtraList.Add(new TourExtra(reader));
                }

                reader.NextResult();
                while (reader.Read())
                {
                    tourPriceBreakdownList.Add(new TourPriceBreakdown(reader));
                }

                reader.NextResult();
                if (reader.Read())
                    tourScheduleObject = new TourSchedule(reader);

                reader.NextResult();
                while (reader.Read())
                {
                    tourVendorPromoList.Add(new TourVendorPromo(reader));
                }

                reader.NextResult();
                while (reader.Read())
                {
                    guiderExcludedDatesList.Add(new GuiderExcludedDates(reader));
                }

                reader.NextResult();
                while (reader.Read())
                {
                    userTourBookingList.Add(new UserTourBooking(reader));
                }

                reader.NextResult();
                while (reader.Read())
                {
                    userList.Add(new User(reader));
                }
            }
        }
        #endregion
    }
}