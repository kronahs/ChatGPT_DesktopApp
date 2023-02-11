using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chatGPT
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            pbResponse.Visible = false;

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            // Define the prompt
            
        }

        static string GenerateResponse(string apiEndpoint, string apiKey, string prompt)
        {
            using (var client = new HttpClient())
            {
                // Set the API key in the request header
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiKey);

                // Define the request body
                var requestBody = new StringContent(JsonConvert.SerializeObject(new
                {
                    prompt = prompt,
                    max_tokens = 1024,
                    n = 1,
                    temperature = 0.5
                }), Encoding.UTF8, "application/json");

                // Send the API request
                var response = client.PostAsync(apiEndpoint, requestBody).Result;
                response.EnsureSuccessStatusCode();

                // Get the response body
                var responseBody = response.Content.ReadAsStringAsync().Result;

                // Parse the response body
                dynamic responseJson = JsonConvert.DeserializeObject(responseBody);

                // Return the generated response
                return responseJson["choices"][0]["text"];
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //pbResponse.Visible = true;
            pbResponse.Minimum = 0;
            pbResponse.Maximum = 100;

            // Start the progress bar
            pbResponse.Value = 0;
            pbResponse.Step = 40;


            string apiEndpoint = "https://api.openai.com/v1/engines/text-davinci-002/completions";
            string apiKey = "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";

            string prompt = txtPrompt.Text;

            // Use the API to generate a response
            string response = GenerateResponse(apiEndpoint, apiKey, prompt);

            // Print the generated response
           
            while(rtbResponse.Text != response)
            {
                pbResponse.PerformStep();
                rtbResponse.Text = response;
            }
             if(rtbResponse.Text != "")
            {
                pbResponse.Visible = false;
            }

            
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private  async void copyText(object sender, EventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(rtbResponse.Text);
            lblHint.Text = "Text Copied";
             await System.Threading.Tasks.Task.Delay(6000);
            lblHint.Text = "";
        }
    }
}
