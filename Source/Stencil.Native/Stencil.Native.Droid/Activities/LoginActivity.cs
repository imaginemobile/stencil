using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Stencil.Native.Droid.Core;
using Stencil.Native.ViewModels;
using Android.Content.PM;
using Stencil.Native.Core;
using Stencil.SDK;
using Stencil.SDK.Models.Responses;
using Stencil.SDK.Models.Requests;

namespace Stencil.Native.Droid.Activities
{
    [Activity(Label = "@string/app_name", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/Theme.Stencil")]
    public class LoginActivity : BaseActivity, IViewModelView
    {
        public LoginActivity()
            : base("LoginActivity")
        {
        }

        #region Controls

        protected EditText txtUser { get { return this.GetControl<EditText>(Resource.Id.general_user); } }
        protected EditText txtPassword { get { return this.GetControl<EditText>(Resource.Id.general_password); } }
        protected Button btnLogin { get { return this.GetControl<Button>(Resource.Id.general_button); } }

        #endregion

        #region Properties

        public LoginViewModel ViewModel { get; set; }

        #endregion

        #region Overrides

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.ExecuteMethod("OnCreate", delegate ()
            {
                base.OnCreate(savedInstanceState);

                this.ViewModel = new LoginViewModel(this);

                this.SetContentView(Resource.Layout.Login);

                btnLogin.Click += btnLogin_Click;
                txtUser.Hint = this.ViewModel.Text_General_EmailWatermark;
                txtPassword.Hint = this.ViewModel.Text_General_PasswordWatermark;
                btnLogin.Text = this.ViewModel.Text_SignIn;

                OnReturnExecute(txtPassword, DoLogin);

                this.ViewModel.Start();
            });
        }

        protected override void OnResume()
        {
            base.ExecuteMethod("OnResume", delegate ()
            {
                base.OnResume();

                this.ViewModel.OnAppear();
                
                Container.ViewPlatform.ShowOutDatedMessageIfNeeded(this);
            });
        }

        protected override void OnPause()
        {
            base.ExecuteMethod("OnPause", delegate ()
            {
                base.OnPause();

                this.ViewModel.OnDisappear();
            });
        }

        #endregion

        #region Protected Methods

        protected void DoLogin()
        {
            base.ExecuteMethodAsync("DoLogin", async delegate ()
            {
                this.HideKeyboard();

                if(!this.IsValid())
                {
                    return; // pre-validate before we start processing
                }

                HUD.Show(this, this.ViewModel.Text_LoggingIn);

                ActionResult response = await this.ViewModel.LoginAsync(txtUser.Text, txtPassword.Text);

                if(response.IsSuccess())
                {
                    HUD.Dismiss();
                    this.StartActivity<PostsActivity>(true, true);
                }
                else
                {
                    HUD.ShowErrorWithStatus(this, response.GetMessage(), 2800);
                }
            });
        }

        protected bool IsValid()
        {
            return base.ExecuteFunction("IsValid", delegate ()
            {
                string errorMessage = this.ViewModel.Validate(txtUser.Text, txtPassword.Text);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    HUD.ShowErrorWithStatus(this, errorMessage, 1800);
                    return false;
                }
                return true;
            });
        }
       
        #endregion

        #region Event Handlers


        public void btnLogin_Click(object sender, EventArgs e)
        {
            this.DoLogin();
        }

        #endregion
    }
}