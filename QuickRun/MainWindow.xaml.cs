﻿using System;
using System.IO;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuickRun
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadingScreen();
            MiniAppWindow openmini = new MiniAppWindow();
            openmini.Show();
        }
        

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
            Keyboard.ClearFocus();
        }

        private void Buttonclose(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Buttonminimise(object sender, RoutedEventArgs e)
        {
            MainPage.WindowState = WindowState.Minimized;
        }

        public string itemname = "";
        public string itemtype = "";
        public string itemlink = "";
        public string itemshortcut = "";

        private void itemnamemethod(object sender, RoutedEventArgs e)
        {
            string itemnametemp = (string)((Button)sender).Tag;

            string[] readText = File.ReadAllLines(@"C:\test\test.txt");
            foreach (string text in readText)
            {
                string myString = text;
                string[] mon = myString.Split('=');   //We're making array for mon[0] is name and mon[1] is shortcut

                if(mon[0] == itemnametemp)
                {
                    itemtype = mon[1];
                    itemlink = mon[2];
                    itemshortcut = mon[3];
                }
            }

            itemname = itemnametemp;
        }

        private void Resultpagemethod(object sender, RoutedEventArgs e)
        {
            ResultPage dashboard = new ResultPage(itemname, itemlink, itemtype, itemshortcut);
            dashboard.Owner = Window.GetWindow(this);
            dashboard.AppMainWindow = this;
            ShowResultPanel.Content = null;
            ShowResultPanel.Content = dashboard;
        }

        public MainWindow AppMainWindow { get; set; }
        public void Editpagemethod(object sender, RoutedEventArgs e)
        {
            EditPage newpagedashboard = new EditPage(itemname, itemlink, itemtype, itemshortcut);
            newpagedashboard.Owner = Window.GetWindow(this);
            newpagedashboard.AppMainWindow = this;
            EditPanel.Content = newpagedashboard;
        }
        public void DeleteItem(object sender, RoutedEventArgs e)
        {
            //We're creating a new array finalText to store new set of itemlist
            string[] readText = File.ReadAllLines(@"C:\test\test.txt");
            string[] finalText = new string[readText.Length - 1];

            /*Google-Website-www.google.com-Shift+G
            Premiere Pro-Software-C/PremierePro/Adobe/Pro.exe-Shift+P
            LinkedIn-Folder-C/Windows/Folders/Myfolder-Shift+L
            Amazon-Other-www.amazon.com-Shift+A
            Test File-Folder-C:\test\test.txt-Shift+T*/
            int j = 0;
            var fintext = new List<string>();
            for (int i = 0; i < readText.Length; i++)
            {
                string[] mon = readText[i].Split('='); //We're making array for mon[0] is name and mon[1] is shortcut

                if(mon[0] != itemname)
                {
                    fintext.Add(readText[i]);
                }
            }

            File.WriteAllLines(@"C:\test\test.txt", fintext);
            LoadingScreen();

            MenuPopUp(itemname + " is deleted!");
        }

        public async Task MenuPopUp(string Text)
        {
            await MenuPopUp1(Text);
            await MenuPopUp2(Text);
        }

        public async Task MenuPopUp1(string text)  //Animation first part
        {
            popuptext.Content = text;
            TranslateTransform trans = new TranslateTransform();
            popupbar.RenderTransform = trans;
            DoubleAnimation anim1 = new DoubleAnimation(0, -50, TimeSpan.FromSeconds(0.7));
            trans.BeginAnimation(TranslateTransform.YProperty, anim1);

            //The delay between the two animations
            await Task.Delay(5 * 1000);
        }
        public async Task MenuPopUp2(string text)  //Animation Second part
        {
            popuptext.Content = text;
            TranslateTransform trans = new TranslateTransform();
            popupbar.RenderTransform = trans;
            DoubleAnimation anim2 = new DoubleAnimation(-50, 0, TimeSpan.FromSeconds(0.7));
            trans.BeginAnimation(TranslateTransform.YProperty, anim2);

        }


        public bool IsEditOn = false;

        public string[] readText;

        public bool firsttime = true; //For animation in the beginning
        public async void LoadingScreen()
        {
            readText = File.ReadAllLines(@"C:\test\test.txt");

            

            int numberofwebsites = 0;
            int numberofsoftwares = 0;
            int numberoffolders = 0;
            int numberofothers = 0;

            MyPanel.Children.Clear();
            foreach (string text in readText)
            {
                string myString = text;
                string[] mon = myString.Split('=');   //We're making array for mon[0] is name and mon[1] is shortcut

                StackPanel insidebutton = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                };
                Label itemname = new Label() //Item title
                {
                    Width = 120,
                    Content = mon[0], //text of name
                    Tag = mon[0],
                    HorizontalAlignment = HorizontalAlignment.Left,
                    HorizontalContentAlignment = HorizontalAlignment.Left,
                    Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0)),
                };
                Label itemname2 = new Label() //Item shortcut
                {
                    Width = 140,
                    Content = mon[3],
                    HorizontalAlignment = HorizontalAlignment.Right,
                    HorizontalContentAlignment = HorizontalAlignment.Right,
                    Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0)),
                 };
                System.Windows.Shapes.Path editlogo = new System.Windows.Shapes.Path()
                {
                    Width = 13.866,
                    Height = 11.555
                };
                Canvas itemeditblock = new Canvas() { Width = 50 };
                Ellipse typecolor = new Ellipse()
                {
                    Height = 5,
                    Width = 5,
                };

                string type = mon[1]; 

                switch (type)
                {
                    case "Website":
                        typecolor.Fill = new SolidColorBrush(Color.FromArgb(255, 12, 255, 210));
                        numberofwebsites += 1;
                        break;

                    case "Software":
                        typecolor.Fill = new SolidColorBrush(Color.FromArgb(255, 12, 151, 255));
                        numberofsoftwares += 1;
                        break;

                    case "Folder":
                        typecolor.Fill = new SolidColorBrush(Color.FromArgb(255, 252, 169, 3));
                        numberoffolders += 1;
                        break;

                    case "Other":
                        typecolor.Fill = new SolidColorBrush(Color.FromArgb(255, 232, 94, 94));
                        numberofothers += 1;
                        break;
                }  //for typecolor in itembox

                Button namebutton = new Button()
                {
                    Content = insidebutton,
                    BorderThickness = new Thickness(0.5),
                    Background = new SolidColorBrush(Color.FromArgb(125, 250, 249, 247)),
                    BorderBrush = new SolidColorBrush(Color.FromArgb(255, 186, 186, 186)),
                    Margin = new Thickness(5),
                    Width = 280,
                    Height = 40,
                    Tag = mon[0],
                    Style = (Style)FindResource("RoundedButtonStyle")
                    
                 };
                namebutton.Click += itemnamemethod;

                if(IsEditOn == false && IsDeleteOn == false)
                {
                    namebutton.Click += Resultpagemethod;
                }
                
                if(IsEditOn == true)
                {
                    namebutton.Click += Editpagemethod;
                    namebutton.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 52, 149, 235));
                    namebutton.BorderThickness = new Thickness(1);
                    IsDeleteOn = false;
                }

                
                insidebutton.Children.Add(typecolor);
                insidebutton.Children.Add(itemname);
                insidebutton.Children.Add(itemname2);

                //Final display of the items
                if(sortfor == "")
                {
                    MyPanel.Children.Add(namebutton);
                    if (readText.Length >= 6)
                    {
                        namebutton.Width = 270;
                    }
                    if (firsttime == true)
                    {
                        var animation = new DoubleAnimation
                        {
                            From = 0,
                            To = 1,
                            Duration = TimeSpan.FromSeconds(0.5),
                            FillBehavior = FillBehavior.Stop
                        };
                        namebutton.BeginAnimation(Button.OpacityProperty, animation);
                        await Task.Delay(3 * 100);
                    }
                }
                
                if(sortfor == mon[1])
                {
                    MyPanel.Children.Add(namebutton);
                    
                }

                recenttitle.Text = mon[0];
                recentshortcut.Text = mon[3];


                if (IsDeleteOn)
                {
                    namebutton.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 204, 30, 14));
                    namebutton.BorderThickness = new Thickness(1);
                    IsEditOn = false;
                    namebutton.Click += DeleteItem;
                }

                //for category number count
                websitecount.Text = Convert.ToString(numberofwebsites);
                softwarecount.Text = Convert.ToString(numberofsoftwares);
                foldercount.Text = Convert.ToString(numberoffolders);
                othercount.Text = Convert.ToString(numberofothers);


                
            }

            //To finish animation
            firsttime = false;
        }


        public string sortfor = "";
        private void ButtonSort(object sender, RoutedEventArgs e)
        {
            
            string buttonText = ((Button)sender).Name;
            switch (buttonText)
            {
                case "buttonwebsite":
                    sortfor = "Website";
                    break;
                case "buttonsoftware":
                    sortfor = "Software";
                    break;
                case "buttonfolder":
                    sortfor = "Folder";
                    break;
                case "buttonother":
                    sortfor = "Other";
                    break;
            }

            LoadingScreen();
        }

        private void Dashboard(object sender, RoutedEventArgs e)
        {
            sortfor = "";
            LoadingScreen();
        }

        private void AddNew(object sender, RoutedEventArgs e)
        {
                AddNewPage newpagedashboard = new AddNewPage();
                newpagedashboard.Owner = this;
                newpagedashboard.AppMainWindow = this;
                AddNewPanel.Content = newpagedashboard;
        }

        public void Edit(object sender, RoutedEventArgs e)
        {
            while (true)
            {
                if (!IsEditOn) {
                    buttonedit.Background = new SolidColorBrush(Color.FromArgb(255, 145, 220, 255));
                    IsEditOn = true;
                    LoadingScreen();
                    break;
                }
                if(IsEditOn)
                {
                    buttonedit.Background = new SolidColorBrush(Color.FromArgb(255, 250, 249, 247));
                    IsEditOn = false;
                    LoadingScreen();
                    break;
                }
            }
        }

        public bool IsDeleteOn = false;
        private void Delete(object sender, RoutedEventArgs e)
        {
            while (true)
            {
                if (!IsDeleteOn)
                {
                    buttondelete.Background = new SolidColorBrush(Color.FromArgb(255, 204, 30, 14));
                    deletetext.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    deletelogo.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    IsDeleteOn = true;
                    LoadingScreen();
                    break;
                }
                if (IsDeleteOn)
                {
                    buttondelete.Background = new SolidColorBrush(Color.FromArgb(255, 250, 249, 247));
                    deletetext.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                    deletelogo.Fill = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                    IsDeleteOn = false;
                    LoadingScreen();
                    break;
                }
            }

        }

        public void GetInstalledApps()
        {
            string uninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(uninstallKey))
            {
                foreach (string skName in rk.GetSubKeyNames())
                {
                    using (RegistryKey sk = rk.OpenSubKey(skName))
                    {
                        try
                        {
                            Console.WriteLine(sk.GetValue("DisplayName"));
                        }
                        catch (Exception ex)
                        { }
                    }
                }
            }
        }

        private void OpenMini(object sender, RoutedEventArgs e)
        {
            MiniAppWindow opennewmini = new MiniAppWindow();
            this.Close();
            opennewmini.ShowDialog();
        }

        private void openmyfacebook(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.facebook.com/mohammed.zishan.338/");
        }

        private void openmygithub(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/MohdZish");
        }

        private void openmyyoutube(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.youtube.com/channel/UCjK0Oq2XAo-oBEoytkSmYAg");
        }

        private void Recent(object sender, RoutedEventArgs e)
        {
            if (recenttitle.Text != "Add your first")
            {

                string itemnametemp = recenttitle.Text;

                string[] readText = File.ReadAllLines(@"C:\test\test.txt");
                foreach (string text in readText)
                {
                    string myString = text;
                    string[] mon = myString.Split('=');   //We're making array for mon[0] is name and mon[1] is shortcut

                    if (mon[0] == itemnametemp)
                    {
                        itemtype = mon[1];
                        itemlink = mon[2];
                        itemshortcut = mon[3];
                    }
                }

                ResultPage dashboard = new ResultPage(itemnametemp, itemlink, itemtype, itemshortcut);
                dashboard.Owner = Window.GetWindow(this);
                dashboard.AppMainWindow = this;
                ShowResultPanel.Content = null;
                ShowResultPanel.Content = dashboard;
            }

            else
            {
                AddNewPage newpagedashboard = new AddNewPage();
                newpagedashboard.Owner = this;
                newpagedashboard.AppMainWindow = this;
                AddNewPanel.Content = newpagedashboard;
            }
        }

    }
}
