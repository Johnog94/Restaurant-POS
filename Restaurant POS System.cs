using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
//*Year:1MIS1 Masters of science (information systems managment)
//*Course:Business Applications Programming MS806
//*Name:Jonathan Griffey
//*Id No.:13304011
//*Assignment 5: Da Cheeky Cow




namespace GRIFFEY_JONATHAN_ASSIGNMENT5_MS806
{
    public partial class FormDaCheekyCow : Form
    {
        //this is a 2d array with prices from excel file
        readonly static string[] MEAL_OPTION = {"Full Irish", "Irish Grill", "Bellmullet Grill", "Curry Special", "Irish Stew",
            "Student Stew","Bacon & Cabbage", "Colcannon", "Boxty Special", "Atlantic Way","Coddle", "Snack Box" };
        readonly static string[] MEAL_SIZE = { "Leprechuan", "Child", "Adult", "Student", "Cuchulainn" };
        readonly decimal[,] PRICES = {{ 1.23m,  2.40m,  3.49m,  2.19m,  3.99m,  7.89m,  3.45m,  3.67m,  4.55m,  5.00m,  1.23m,  3.45m,  },
                                      {5.69m,   2.90m,  3.99m,  3.12m,  4.49m,  8.89m,  3.75m,  3.97m,  5.00m,  5.50m,  2.23m,  3.95m, },
                                      {6.83m,   3.40m,  4.49m,  4.05m,  5.50m,  8.50m,  4.25m,  4.50m,  5.36m,  6.00m, 3.23m,   4.45m, },
                                      {6.99m,   3.59m,  4.99m,  4.49m,  6.51m,  8.11m,  4.75m,  5.03m,  5.72m,  6.50m,  4.23m,  4.95m,},
                                      {9.99m,   3.78m,  5.49m,  4.93m,  7.52m,  7.72m,  5.25m,  5.56m,  6.08m,  7.00m,  5.23m,  5.45m, } };


        //declare global variables
        string meal, size;
        decimal mealPrice;
        //stock file
        string DCC_Stocks = "DCC_Stocks.txt";
        //temp arrays set up for reading stock file
        int[,] tempStocks = new int[5, 12];
        int[,] tempstock = new int[5, 12];
        decimal total_Price;
        string trx_log;
        






        public FormDaCheekyCow()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            //disenales transcation log list box
            trxLogListBox.Enabled = false;
            //make complete order buton disenabled 
            completeButton.Enabled = false;
            //date and time added to form with timer to have working time
            timer1.Start();
            timeLabel.Text = DateTime.Now.ToLongTimeString();
            dateLabel.Text = DateTime.Now.ToLongDateString();


            int i = 0;
            int j = 0;
            // following code is the method that reads stock file from start to see available stock for each item
            var reader = new StreamReader(DCC_Stocks);
            //try/catch with exception to allow it read stock file
            try
            {




                while (!reader.EndOfStream)
                {
                    for (i = 0; i < 5; i++)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split('\t');
                        for (j = 0; j < 12; j++)
                        {
                            tempStocks[i, j] = int.Parse(values[j]);
                        }
                        {


                        }
                    }




                }
                reader.Close();




            }
            catch (FormatException exc)
            {


            }
        }


        


        private void timer1_Tick(object sender, EventArgs e)
        {
            //timer for clock
            timeLabel.Text = DateTime.Now.ToLongTimeString();
            timer1.Start();
        }


        private void mealsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int meal_name = mealsListBox.SelectedIndex;
            int meal_size = sizesListBox.SelectedIndex;
            //if statement allowing prices,meals and sizes to be printed to associated labels
            if (meal_name != -1 && meal_size != -1)
            {


                meal = MEAL_OPTION[meal_name];
                size = MEAL_SIZE[meal_size];
                mealPrice = PRICES[meal_size, meal_name];
                priceInputLabel.Text = mealPrice.ToString("C");
                mealInputLabel.Text = meal;
                sizeInputLabel.Text = size;


            }
            
            
        }


        private void sizesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int meal_name = mealsListBox.SelectedIndex;
            int meal_size = sizesListBox.SelectedIndex;
            //if statement allowing prices,meals and sizes to be printed to associated labels
            if (meal_name != -1 && meal_size != -1)
            {
                
                    meal = MEAL_OPTION[meal_name];
                    size = MEAL_SIZE[meal_size];
                    mealPrice = PRICES[meal_size, meal_name];
                    priceInputLabel.Text = mealPrice.ToString("C");
                    mealInputLabel.Text = meal;
                    sizeInputLabel.Text = size;


            }
          
            
            




        }


        private void orderButton_Click(object sender, EventArgs e)
        {
            //declare local variables
            int quantity = 0;
            int stock;
            int mealindex = mealsListBox.SelectedIndex;
            int sizeindex = sizesListBox.SelectedIndex;
            int stock_level;
            int row, col;


            if (mealsListBox.SelectedIndex == -1 && sizesListBox.SelectedIndex == -1)
            {
                //error message to prompt to user to select meal or size if nothing in list boxes selected
                MessageBox.Show("Please Select Meal & Size","Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            try
            {
                
                
                quantity = int.Parse(quantityTextBox.Text);




                stock = tempStocks[sizeindex, mealindex];
                //for loop to read stock file
                for (row = 0; row < 5; row++)
                {
                    for (col = 0; col < 12; col++)
                    {
                        tempstock[row, col] = tempstock[row, col];
                    }
                }


                if (stock < quantity)
                {
                    //message box to show user what stock levels are left
                    MessageBox.Show("only " + stock + " In Stock", "Stocks", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //reset focus to quantity text box with the available quantity
                    quantityTextBox.Focus();
                    quantityTextBox.Text = stock.ToString("");
                }
                else
                {
                    //if suffiecent stock the following code takes stock from whats there and prints it by format with spacing to the order display rich text box
                    decimal totalLineItem;
                    totalLineItem = quantity * mealPrice;
                    stock_level = stock - quantity;
                    tempStocks[sizeindex, mealindex] = stock_level;
                    orderDisplayBox.AppendText(string.Format("{0} | {1} | {2} | {3}, \n", meal.ToString(), size.ToString(), quantity.ToString(),
                        totalLineItem.ToString("C")));
                    total_Price = total_Price + totalLineItem;
                    totalPriceLabel.Text = total_Price.ToString("C");


                }
            }
            catch
            {
                //error message if no quantity inputed
                MessageBox.Show("Please Enter Quantity Amount ", "Error", MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            //complete order button enabled
            completeButton.Enabled = true;


        }


        private void completeButton_Click(object sender, EventArgs e)
        {
            //Building Transaction String - Displays random transaction number in reciept
            string IDNumber = System.Guid.NewGuid().ToString("N").Substring(1, 10);
            //local variables
            string allOrder = orderDisplayBox.Text;
            string time = timeLabel.Text;
            string date = dateLabel.Text;


            //this prompts user to confirm if they wish to proceed with current order
            var result = MessageBox.Show("Are you Sure You Wish To Complete Order?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
                //if result is yes, a formatted reciept with all details is produced
            {
                //formatted reciept
                MessageBox.Show("Your Order has Been Confirmed" + "\n"
                    + "Time: " + timeLabel.Text + "\n"
                    + "Date: " + dateLabel.Text + "\n"
                    + "Transaction Number: " + IDNumber + "\n"
                    + "Choice: " + "\n" + allOrder + "\n"
                    + "Total: " + total_Price.ToString("C") + "\n"
                    , "Receipt");


                //this code prints details from reciept to transaction log listbox
                trxLogListBox.Items.Add("Time: " + timeLabel.Text + "\n");
                trxLogListBox.Items.Add("Date: " + dateLabel.Text + "\n");
                trxLogListBox.Items.Add("Choice: " + "\n" + allOrder + "\n");
                trxLogListBox.Items.Add("Total: " + total_Price.ToString("C") + "\n"); 
                trxLogListBox.Items.Add("--------------------------------------------------------------------------------------------------");
                
                //creates a string to allow data to be held for associated box
                StringBuilder Transactions = new StringBuilder();
                foreach (object selecteditem in trxLogListBox.Items)
                {
                    Transactions.AppendLine(selecteditem.ToString());
                    trx_log = Transactions.ToString();
                }
                
                
                //following code clears form to start when order is completed
                priceInputLabel.Text = " ";
                mealsListBox.ClearSelected();
                sizesListBox.ClearSelected();
                quantityTextBox.Clear();
                mealInputLabel.Text = "";
                sizeInputLabel.Text = "";
                totalPriceLabel.Text = "";
                orderDisplayBox.Clear();
                total_Price = 0;




                
            }
            
            
        }


        private void clearButton_Click(object sender, EventArgs e)
        {
            //this code clears form to start so user can change or cancel order
            priceInputLabel.Text = " ";
            mealsListBox.ClearSelected();
            sizesListBox.ClearSelected();
            quantityTextBox.Clear();
            mealInputLabel.Text = "";
            sizeInputLabel.Text = "";
            totalPriceLabel.Text = "";
            orderDisplayBox.Clear();
            completeButton.Enabled = false;




        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //prompts user with message to confirm exit
            var result = MessageBox.Show("Are you Sure You Wish To Exit Programme?", "Exit", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                //if exit confirmed, transactions saved to text file and application closed
                System.IO.StreamWriter transactionsummary;
                transactionsummary = new System.IO.StreamWriter("Transactions" +
                    DateTime.Today.Year.ToString() + "_" +
                    DateTime.Today.Month.ToString() + "_" +
                    DateTime.Today.Day.ToString() + ".txt");
                transactionsummary.Write(trx_log);
                transactionsummary.Close();
                this.Close();
            }
                
        }


        private void stockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //opens stock text file for viewing
            Process.Start("notepad.exe", DCC_Stocks);
        }


        private void transactionLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //shows transactions in transactions tab
            tabControl.SelectedTab = transactionPage;
        }


        private void exitButton_Click(object sender, EventArgs e)
        {


            
            //this writes all transactions to text file and saves
            System.IO.StreamWriter transactionsummary;
            transactionsummary = new System.IO.StreamWriter("Transactions" +
                DateTime.Today.Year.ToString() + "_" +
                DateTime.Today.Month.ToString() + "_" +
                DateTime.Today.Day.ToString() + ".txt");
            transactionsummary.Write(trx_log);
            transactionsummary.Close();


            //application close
            this.Close();
        }






    }
}
