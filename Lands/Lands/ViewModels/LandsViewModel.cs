

namespace Lands.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System.Collections.ObjectModel;
    using Models;
    using Services;
    using System;
    using Xamarin.Forms;
    using System.Collections.Generic;

    public class LandsViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiServices;
        #endregion


        #region Attributes
        private ObservableCollection<Land> lands;
        #endregion

        #region Properties
        public ObservableCollection<Land> Lands
        {
            get { return this.lands; }
            set { SetValue(ref lands, value); }
        }
        #endregion

        #region Constructors
        public LandsViewModel()
        {
            this.apiServices = new ApiService();
            this.LoadLands();
        }
        #endregion

        #region Methods
        private async void LoadLands()
        {
            var connection = await this.apiServices.CheckConnection();

            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Accept");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }


            var response = await this.apiServices.GetList<Land>(
                "http://restcountries.eu", 
                "/rest", 
                "/v2/all");

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }
            var list = (List<Land>)response.Result;
            this.Lands = new ObservableCollection<Land>(list);

        }
        #endregion


    }
}
