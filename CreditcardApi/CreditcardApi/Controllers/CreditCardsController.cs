using CreditcardApi.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CreditcardApi.Controllers
{
    public class CreditCardsController : ApiController
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        // GET: api/CreditCards
        [HttpGet]
        [Route("api/creditcards")]
        public IEnumerable<CreditCard> GetCreditCards()
        {
            List<CreditCard> creditCards = new List<CreditCard>();

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM creditcardtbl", connection))
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CreditCard card = new CreditCard
                        {
                            paymentdetailid = Convert.ToInt32(reader["paymentDetailId"]),
                            cardownername = reader["cardOwnerName"].ToString(),
                            cardnumber = reader["cardNumber"].ToString(),
                            expirationdate = reader["expirationDate"].ToString(),
                            securitycode = reader["securityCode"].ToString()
                        };
                        creditCards.Add(card);
                    }
                }
            }

            return creditCards;
        }
        // GET: api/CreditCards/5
        [HttpGet]
        [Route("api/creditcards/{paymentDetailId}")]
        public IHttpActionResult GetCreditCard(int paymentDetailId)
        {
            CreditCard card = null;

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM creditcardtbl WHERE paymentdetailid = @paymentdetailid", connection))
                {
                    command.Parameters.AddWithValue("@paymentdetailid", paymentDetailId);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            card = new CreditCard
                            {
                                paymentdetailid = Convert.ToInt32(reader["paymentdetailid"]),
                                cardownername = reader["cardownername"].ToString(),
                                cardnumber = reader["cardnumber"].ToString(),
                                expirationdate = reader["expirationdate"].ToString(),
                                securitycode = reader["securitycode"].ToString()
                            };
                        }
                    }
                }
            }

            if (card == null)
            {
                return NotFound(); // Trả về HTTP 404 nếu không tìm thấy
            }

            return Ok(card); // Trả về dữ liệu CreditCard nếu tìm thấy
        }
        [HttpPost]
        [Route("api/creditcards")]
        public IHttpActionResult PostCreditCard([FromBody] CreditCard card)
        {
            //using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            //{
            //    connection.Open();

            //    using (NpgsqlCommand command = new NpgsqlCommand("INSERT INTO creditcardtbl (cardownername, cardnumber, expirationdate, securitycode) VALUES (@OwnerName, @CardNumber, @ExpirationDate, @SecurityCode)", connection))
            //    {
            //        command.Parameters.AddWithValue("@OwnerName", card.cardownername);
            //        command.Parameters.AddWithValue("@CardNumber", card.cardnumber);
            //        command.Parameters.AddWithValue("@ExpirationDate", card.expirationdate);
            //        command.Parameters.AddWithValue("@SecurityCode", card.securitycode);
            //        int rowsAffected = command.ExecuteNonQuery();
            //    }
            //}
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO creditcardtbl (cardownername, cardnumber, expirationdate, securitycode) VALUES (@OwnerName, @CardNumber, @ExpirationDate, @SecurityCode)";

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OwnerName", card.cardownername);
                        command.Parameters.AddWithValue("@CardNumber", card.cardnumber);
                        command.Parameters.AddWithValue("@ExpirationDate", card.expirationdate);
                        command.Parameters.AddWithValue("@SecurityCode", card.securitycode);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return Ok("Credit card information inserted successfully.");
                        }
                        else
                        {
                            return BadRequest("Failed to insert credit card information.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi ở đây hoặc ghi log lỗi
                return InternalServerError(ex);
            }
        }


        // PUT: api/CreditCards/5
        [HttpPut]
        [Route("api/creditcards/{id}")]
        public IHttpActionResult PutCreditCard(int id, [FromBody] CreditCard card)
        {
            //using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            //{
            //    connection.Open();

            //    using (NpgsqlCommand command = new NpgsqlCommand("UPDATE creditcardtbl SET cardOwnerName = @OwnerName, cardNumber = @CardNumber, expirationDate = @ExpirationDate, securityCode = @SecurityCode WHERE paymentDetailId = @Id", connection))
            //    {
            //        command.Parameters.AddWithValue("@OwnerName", card.cardownername);
            //        command.Parameters.AddWithValue("@CardNumber", card.cardnumber);
            //        command.Parameters.AddWithValue("@ExpirationDate", card.expirationdate);
            //        command.Parameters.AddWithValue("@SecurityCode", card.securitycode);
            //        command.Parameters.AddWithValue("@Id", id);
            //        command.ExecuteNonQuery();
            //    }
            //}
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "UPDATE creditcardtbl SET cardOwnerName = @OwnerName, cardNumber = @CardNumber, expirationDate = @ExpirationDate, securityCode = @SecurityCode WHERE paymentDetailId = @Id";

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OwnerName", card.cardownername);
                        command.Parameters.AddWithValue("@CardNumber", card.cardnumber);
                        command.Parameters.AddWithValue("@ExpirationDate", card.expirationdate);
                        command.Parameters.AddWithValue("@SecurityCode", card.securitycode);
                        command.Parameters.AddWithValue("@Id", id);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return Ok("Credit card information updated successfully.");
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi ở đây hoặc ghi log lỗi
                return InternalServerError(ex);
            }
        }

        // DELETE: api/CreditCards/5
        [HttpDelete]
        [Route("api/creditcards/{id}")]
        public IHttpActionResult DeleteCreditCard(int id)
        {
            //using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            //{
            //    connection.Open();

            //    using (NpgsqlCommand command = new NpgsqlCommand("DELETE FROM creditcardtbl WHERE paymentDetailId = @Id", connection))
            //    {
            //        command.Parameters.AddWithValue("@Id", id);

            //        command.ExecuteNonQuery();
            //    }
            //}
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "DELETE FROM creditcardtbl WHERE paymentDetailId = @Id";

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return Ok("Credit card information deleted successfully.");
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi ở đây hoặc ghi log lỗi
                return InternalServerError(ex);
            }
        }
    }
}
