using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains path for api
/// </summary>
public static class ApiPathManager
{

    /// <summary>
    /// The url of the server
    /// Localhost: https://localhost:44366
    /// Online:https://scratchking.net
    /// </summary>
    public static string ServerUrl => "https://scratchking.net";

    /// <summary>
    /// The url for the login page
    /// </summary>
    public static string LoginUrl => $"{ServerUrl}/user/login";

    /// <summary>
    /// The url for the signup page
    /// </summary>
    public static string SignupUrl => $"{ServerUrl}/user/signup";

    /// <summary>
    /// Validates the existing token
    /// </summary>
    public static string TokenValidationUrl => $"{ServerUrl}/user/token_validation";

    /// <summary>
    /// The path to get the best players of the game
    /// </summary>
    public static string BestPlayersUrl => $"{ServerUrl}/user/bestplayers";

    /// <summary>
    /// The path to update the earnings of the user
    /// </summary>
    public static string GameDataUpdate => $"{ServerUrl}/user/update_game_data";

    /// <summary>
    /// /The url to claim free tickets
    /// </summary>
    public static string FreeTicketClaimUrl => $"{ServerUrl}/user/claim_free_ticket";

    /// <summary>
    /// The url to update user profile
    /// </summary>
    public static string ProfileUpdateUrl => $"{ServerUrl}/user/update_profile";

    /// <summary>
    /// The url to get the privacy policy
    /// </summary>
    public static string PrivacyPolicyUrl => $"{ServerUrl}/user/privacy_policy";

    /// <summary>
    /// The url for terms of service for the application
    /// </summary>
    public static string TermsOfServiceUrl => $"{ServerUrl}/user/terms_of_service";

    /// <summary>
    /// The url to get whether the user can watch video ad or not
    /// </summary>
    public static string VideoAdsUrl => $"{ServerUrl}/user/watch_video_ads";

    /// <summary>
    /// The url to request the redeem withdrawl
    /// </summary>
    public static string RedeemRequestUrl => $"{ServerUrl}/user/redeem_request_handler";

    /// <summary>
    /// The url to reset the username or password
    /// </summary>
    public static string ForgetUrl => $"{ServerUrl}/user/forget";

    /// <summary>
    /// The url to get list of countries from the server
    /// </summary>
    public static string CountriesListUrl => $"{ServerUrl}/user/countries";

    /// <summary>
    /// The url to get the application url
    /// </summary>
    public static string ApplicaitonLinkUrl => $"{ServerUrl}/user/application_link";

    /// <summary>
    /// Generates username and password for new user
    /// </summary>
    public static string NewUserGenerationUrl => $"{ServerUrl}/user/generate_userInformation";

}
