using Assets.Scripts;
using Assets.Scripts.Services;
using Assets.Scripts.Shared;
using Assets.Scripts.Shared.GameDatas;
using Assets.Scripts.Shared.Games;
using Assets.Scripts.UIs;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
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
    public GameStateUI ui;

    public TMP_InputField LoginUsernameInputfield;

    public Button LoginButton;
    public Button LogoutButton;
    public Button GameStartButton;
    public Button SelectNpcButton;
    public Button SelectCardButton;
    public Button PowerUpButton;
    public Button NextFloorButton;
    public Button BattleEndButton;

    LoginService _loginService = new();
    UserService _userService = new();
    RequestMissionService _requestMissionService = new();
    CharacterService _characterService = new();
    ServerSandBoxService _serverSandBoxService = new();
    AchievementService _achievementService = new();
    GameService _gameService = new();

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

        GameStartButton.onClick.AddListener(() => GameStateStart());
        SelectNpcButton.onClick.AddListener(() => SelectNPC());
        SelectCardButton.onClick.AddListener(() => SelectCard());
        PowerUpButton.onClick.AddListener(() => PowerUp());
        NextFloorButton.onClick.AddListener(() => NextFloor());
        BattleEndButton.onClick.AddListener(() => BattleEnd());
        LoginButton.onClick.AddListener(() => Login());
        LogoutButton.onClick.AddListener(() => LogOut());
    }

    async Task GameStateStart()
    {
        var state = await _gameService.Start();
        ShowGameState(state);
        UnityEngine.Debug.Log("게임 시작");
    }

    async Task SelectNPC()
    {
        var state = await _gameService.SelectNPC(0);
        ShowGameState(state);
        UnityEngine.Debug.Log("selectnpc");
    }

    async Task SelectCard()
    {
        var state = await _gameService.SelectCard(0);
        ShowGameState(state);
        UnityEngine.Debug.Log("selectcard");
    }

    async Task PowerUp()
    {
        var state = await _gameService.PowerUp();
        ShowGameState(state);
        UnityEngine.Debug.Log("powerup");
    }

    async Task NextFloor()
    {
        var state = await _gameService.NextFloor();
        ShowGameState(state);
        UnityEngine.Debug.Log("nextFloor");
    }

    async Task BattleEnd()
    {
        var state = await _gameService.BattleEnd();
        ShowGameState(state);
        UnityEngine.Debug.Log("BattleEnd");
    }

    public void ShowGameState(GameState gamestate)
    {
        ui.Set(gamestate);
    }

    async Task LogOut()
    {
        await _loginService.Logout();
    }
    async Task Login()
    {
        var username = LoginUsernameInputfield.text;

        try
        {
            await _loginService.Register(username, "password");
            await Task.Delay(2000);
            await _loginService.Login(username, "password");
            Debug.Log("로그인 완료");
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            return;
        }
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
