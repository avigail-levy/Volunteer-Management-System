using PL.Volunteer;
using System;
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public int Id { get; set; }
        public string Password { get; set; }
        public static readonly DependencyProperty RoleProperty =
        DependencyProperty.Register("Role", typeof(BO.Role), typeof(Login), new PropertyMetadata(BO.Role.Volunteer));

        public BO.Role Role
        {
            get => (BO.Role)GetValue(RoleProperty);
            set => SetValue(RoleProperty, value);
        }
        public Login()
        {
            InitializeComponent();
        }
        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Id == null || Id < 100000000 || Id > 999999999)
                {
                    MessageBox.Show("enter correct id");
                    return;
                }
                if (string.IsNullOrWhiteSpace(Password))
                {
                    MessageBox.Show("enter corrert password");
                    return;
                }

                Role = s_bl.Volunteer.Login(Id, Password);
                if (Role == BO.Role.Volunteer)
                {
                    new VolunteerWindow(Id, Role).Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void mainWindow_Button_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow(Id).Show();
            Close();
        }

        private void volunteerWindow_Button_Click(object sender, RoutedEventArgs e)
        {
            new VolunteerWindow(Id).Show();
            Close();
        }
    }
}