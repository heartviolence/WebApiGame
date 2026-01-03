using Assets.Scripts;
using Assets.Scripts.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class SandBox : MonoBehaviour
{
    HttpClient _client;

    public Button GachaButton;
    public Button CharacterDeleteButton;
    public Button RequestMissionStartButton;
    public Button RequestMissionCompleteButton;
    public Button UserInfoButton;
    public TMP_Text CrystalText;

    LoginService _loginService = new();
    UserService _userService = new();
    RequestMissionService _requestMissionService = new();

    UserInfoDTO _userInfo;

    void Start()
    {
        _client = new HttpClient();
        _client.BaseAddress = new System.Uri("https://localhost:7067/");
        _client.Timeout = TimeSpan.FromSeconds(5);
        GameApiClient.Client = _client;

        GachaButton.onClick.AddListener(() => Gacha());
        CharacterDeleteButton.onClick.AddListener(() => DeleteAll());
        RequestMissionStartButton.onClick.AddListener(() => RequestMissionStart());
        RequestMissionCompleteButton.onClick.AddListener(() => RequestMissionCompleteCheck());
        UserInfoButton.onClick.AddListener(() => UserInfo());
        Login();
    }

    async Task Login()
    {
        await _loginService.Register("admin", "password");
        await _loginService.Login("admin", "password");
        Debug.Log("로그인 완료");
        await UserInfo();
        Show();
    }

    async Task Gacha()
    {
        await _userService.Gacha();
        _userInfo.Characters = await _userService.GetCharacters();
        await UserInfo();
        Show();
    }

    async Task DeleteAll()
    {
        await _userService.DeleteAll();
        _userInfo.Characters = await _userService.GetCharacters();
        Show();
    }

    async Task UserInfo()
    {
        _userInfo = await _userService.GetUserInfo();
        Debug.Log("유저정보 갱신완료");
        CrystalText.text = _userInfo.Crystal.ToString();
    }

    async Task RequestMissionStart()
    {
        await _requestMissionService.RequestMissionStart();
        Debug.Log("미션 시작");
        await UserInfo();
    }

    async Task RequestMissionCompleteCheck()
    {
        await _requestMissionService.RequestMissionCompleteCheck();
        Debug.Log("미션 업데이트완료");
        await UserInfo();
    }
    void Show()
    {
        Debug.Log("캐릭터 정보목록---------------------------");
        foreach (var character in _userInfo.Characters)
        {
            Debug.Log($"Character ID: {character.CharacterID}, Level: {character.Level}, EXP: {character.EXP}");
        }
        Debug.Log("---------------------------");
    }

}
