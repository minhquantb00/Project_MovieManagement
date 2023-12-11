using MovieManagement.DataContext;
using MovieManagement.Entities;

namespace MovieManagement.Handle.HandleEmail
{
    public class BillEmailTemplate
    {
        public static string GenerateNotificationBillEmail(Bill bill, string message = "")
        {
            string htmlContent = $@"
            <html>
            <head>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                    }}
                    image {{
                        width: 60px;
                        height: 70px;
                    }}
                    h1 {{
                        color: #333;
                    }}
                    
                    table {{
                        border-collapse: collapse;
                        width: 100%;
                    }}
                    
                    th, td {{
                        border: 1px solid #ddd;
                        padding: 8px;
                    }}
                    
                    th {{
                        background-color: #f2f2f2;
                        font-weight: bold;
                    }}
                    
                    .footer {{
                        margin-top: 20px;
                        font-size: 14px;
                    }}
                </style>
            </head>
            <body>
                <h1>Thông tin hóa đơn đặt vé</h1>
                <h2 style=""color: red; font-size: 20px; font-weight: bold;"">{(string.IsNullOrEmpty(message) ? "" : message)}</h2>

                <table>
                    <tr>
                        <th>Mã giao dịch</th>
                        <th>Tên hóa đơn</th>
                        <th>Tổng tiền</th>
                        <th>Trạng thái hóa đơn</th>
                        <th>Tên khách hàng</th>
                        <th>Ngày tạo</th>
                    </tr>
                    <tr>
                        <td>{bill.TradingCode}</td>
                        <td>{bill.Name}</td>
                        <td>{bill.TotalMoney}</td>
                        <td>{new AppDbContext().users.SingleOrDefault(x => x.Id == bill.CustomerId).Name}</td>
                        <td>{bill.CreateAt}</td>
                    </tr>
                </table>
                
                <h2>Chi tiết vé</h2>
                <table>
                    <tr>
                        <th>Số lượng vé</th>
                        <th>Tên sản phẩm</th>
                        <th>Số lượng</th>
                        <th>Giá</th>
                    </tr>";

            

            htmlContent += $@"
                       <tr>
                        <td style=""text-align: center;"">{new AppDbContext().billTickets.SingleOrDefault(x => x.BillId == bill.Id).Quantity}</td>
                        <td style=""text-align: center;"">{new AppDbContext().foods.SingleOrDefault(x => x.BillFoods.Any(y => y.BillId == bill.Id)).NameOfFood}</td>
                        <td style=""text-align: center;"">{new AppDbContext().billFoods.SingleOrDefault(x => x.BillId == bill.Id).Quantity}</td>
                        <td colspan=""3"" style=""text-align: center;"">{bill.TotalMoney}</td>
                    </tr>
                </table>
                
                <div class=""footer"">
                    <p>Trân trọng,</p>
                    <p>MyBugs Cinema</p>
                </div>
            </body>
            </html>";

            return htmlContent;
        }
    }
}
