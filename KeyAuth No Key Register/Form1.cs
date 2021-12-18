using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Leaf.xNet;
using Newtonsoft.Json;

namespace KeyAuth_No_Key_Register
{
    public partial class Form1 : Form
    {
        string sellerKey = ""; // put your seller key here!
        static string name = ""; // application name. right above the blurred text aka the secret on the licenses tab among other tabs
        static string ownerid = ""; // ownerid, found in account settings. click your profile picture on top right of dashboard and then account settings.
        static string secret = ""; // app secret, the blurred text on licenses tab and other tabs
        static string version = "1.0"; // leave alone unless you've changed version on website
        public static api KeyAuthApp = new api(name, ownerid, secret, version);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            KeyAuthApp.init(); // will check all the keyauth settings
            Properties.Settings.Default.mySellerKey = sellerKey; // will make the sellerKey = a Setting... we need this so we can input the key.
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                KeyAuthApp.login(textBox1.Text, textBox2.Text); //login with username and password
                MessageBox.Show("Success");
            }
            catch
            {
                MessageBox.Show("Failed");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                genKey(); // starts getting the new key
                string generatedKey = genKey(); // makes the new key equal a string that we will use to register
                KeyAuthApp.register(textBox3.Text, textBox4.Text, generatedKey); // registers with the username, password and new key we made
                MessageBox.Show("Successfully registered! \n\n Key: " + generatedKey); // shows success, with the new key
            }
            catch
            {
                MessageBox.Show("Failed to register");
            }
        }

        static string genKey() // a static string... this is creating / returning the key
        {
            string key = "";
            try
            {
                var req = new HttpRequest(); // starts the request that we will need for the code below
                var keyLink = req.Get("https://keyauth.com/api/seller/?sellerkey=" + Properties.Settings.Default.mySellerKey + "&type=add&expiry=1&mask=XXX-XXX-XXX&level=1&amount=1&format=json");
                // you can change the code on line 69 ^above^ to your liking... only change the "1 after expiry=" ... "XXX-XXX-XXX after mask" ... and the "1 after level" ... 
                string getKey = keyLink.ToString(); // a string to get the generate key (generated on line 69)
                dynamic showKey = JsonConvert.DeserializeObject(getKey); // Converts the json Object to an individual string that we can use to register
                key += showKey.key; // makes the key string equal the actual generated key

                return key; // sends the data (new key)
            }
            catch (HttpException ex)
            {
                return $"Error: {ex}";
            }
        }
    }
}
