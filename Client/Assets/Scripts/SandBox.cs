using Assets.Scripts;
using Assets.Scripts.Shared;
using Assets.Scripts.Shared.GameDatas;
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
    public Button LevelUpButton;
    public Button RankUpButton;
    public Button ShowMetheMoneyButton;
    public Button GainAchievementRewardButton;
    public TMP_Text CrystalText;
    public TMP_Text RankUpText;
    public TMP_Text LevelUpText;

    LoginService _loginService = new();
    UserService _userService = new();
    RequestMissionService _requestMissionService = new();
    CharacterService _characterService = new();
    ServerSandBoxService _serverSandBoxService = new();
    AchievementService _achievementService = new();

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
        LevelUpButton.onClick.AddListener(() => LevelUp());
        RankUpButton.onClick.AddListener(() => RankUp());
        ShowMetheMoneyButton.onClick.AddListener(() => ShowMetheMoney());
        GainAchievementRewardButton.onClick.AddListener(() => GainAchievementRewards());
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
        Show();
        CrystalText.text = _userInfo.Crystal.ToString();
        var rankupItem = _userInfo.GameItems.Where(e => e.Name == ItemNames.CharacterRankUpMaterial).FirstOrDefault();
        RankUpText.text = rankupItem?.Count.ToString() ?? "0";
        var levelupItem = _userInfo.GameItems.Where(e => e.Name == ItemNames.CharacterLevelUpMaterial).FirstOrDefault();
        LevelUpText.text = levelupItem?.Count.ToString() ?? "0";
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

    async Task ShowMetheMoney()
    {
        await _serverSandBoxService.ShowMetheMoney();
        await UserInfo();
    }

    async Task LevelUp()
    {
        await _characterService.UseLevelUpItem(CharacterNames.Sora, 1);
        await UserInfo();
    }
    async Task RankUp()
    {
        await _characterService.RankUp(CharacterNames.Sora);
        await UserInfo();
    }

    async Task GainAchievementRewards()
    {
        await _achievementService.GainAchievementRewards("GachaGameAchievement");
        await UserInfo(); 
    }
    void Show()
    {
        Debug.Log("캐릭터 정보목록---------------------------");
        foreach (var character in _userInfo.Characters)
        {
            Debug.Log($"Character Name: {character.Name}, Level: {character.Level}, EXP: {character.EXP},Rank: {character.Rank}");
        }
        Debug.Log("---------------------------");
    }

}
