﻿using System;
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

namespace PL.Volunteer
{
    /// <summary>
    /// Interaction logic for VolunteerWindow.xaml
    /// </summary>
    public partial class VolunteerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        //public BO.Role Role { get; set; } = BO.Role.Volunteer;

        public VolunteerWindow(int id=0)
        {
            ButtonText = id == 0 ? "Add" : "Update";
            InitializeComponent();

            try
            {
                CurrentVolunteer = id != 0
                    ? s_bl.Volunteer.GetVolunteerDetails(id)
                    : 
                    new BO.Volunteer() {Id=0,Role=BO.Role.Volunteer,Active=false};
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }
        }
        public BO.Volunteer? CurrentVolunteer
        {
            get { return (BO.Volunteer?)GetValue(CurrentVolunteerProperty); }
            set { SetValue(CurrentVolunteerProperty, value); }
        }

        public static readonly DependencyProperty CurrentVolunteerProperty =
            DependencyProperty.Register("CurrentVolunteer", typeof(BO.Volunteer), typeof(VolunteerWindow), new PropertyMetadata(null));
        
        public string ButtonText
        {
            get => (string)GetValue(ButtonTextProperty);
            set => SetValue(ButtonTextProperty, value);
        }

        public static readonly DependencyProperty ButtonTextProperty =
            DependencyProperty.Register("ButtonText", typeof(string), typeof(VolunteerWindow));

        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (ButtonText == "Add")
                s_bl.Volunteer.AddVolunteer(CurrentVolunteer!);
            else
                s_bl.Volunteer.UpdateVolunteerDetails(CurrentVolunteer.Id,CurrentVolunteer);
        }
    }
}
