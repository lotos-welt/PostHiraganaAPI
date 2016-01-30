using UnityEngine;
using LitJson;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class PostHiraganaAPI : MonoBehaviour {

    /// <summary>
    /// GooラボApplicationID.
    /// </summary>
    private const string APPLICATION_ID = "[上記のGithubにアクセス時に取得したIDを利用]";

    /// <summary>
    /// ひらがな化API.
    /// </summary>
    private const string HIRAGANA_API = "https://labs.goo.ne.jp/api/hiragana";

    /// <summary>
    /// ひらがな.
    /// </summary>
    private string m_hiragana;

    /// <summary>
    /// ひらがな変換リクエスト.
    /// </summary>
    /// <param name="Sentence">変換したい文字列.</param>
    /// <param name="ConvertType">変換タイプ（1:ひらがな、2:カタカナ）.</param>
    public IEnumerator HiraganaPostRequest(string Sentence, int ConvertType) {

        // JsonDataの作成.
        JsonData data = new JsonData();

        // アプリケーションID.
        data["app_id"] = APPLICATION_ID;

        // 対象文字列.
        data["sentence"] = Sentence;

        // 変換タイプ（ひらがな、カタカナ）.
        switch(ConvertType){
            case 1:
                data["output_type"] = "hiragana";
                break;
            case 2:
                data["output_type"] = "katakana";
                break;
        }

        string postJsonStr = data.ToJson();

        Debug.Log("send post json: " + postJsonStr);
        
        // bodyを作成.
        byte[] postBytes = Encoding.Default.GetBytes (postJsonStr);
    
        // ヘッダー.
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers["Content-Type"] = "application/json; charaset-UTF8";

        Debug.Log("send post json: " + postJsonStr);

        // リクエストを送信.
        WWW result = new WWW(HIRAGANA_API, postBytes, headers);
        yield return result;

        if (result.error != null) {
            Debug.Log("Post Failure…");
        } else{
            Debug.Log("Post Success!");
            Debug.Log("result: " + result.text);

            JsonData jsonParser = JsonMapper.ToObject(result.text);

            // パース.
            m_hiragana = jsonParser["converted"].ToString();

            Debug.Log(m_hiragana);
        }
    }
}