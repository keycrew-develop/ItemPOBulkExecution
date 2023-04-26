window.saveAsFile = function (fileName, byteBase64) {
    var link = this.document.createElement('a');
    link.download = fileName;
    link.href = "data:application/octet-stream;base64," + byteBase64;
    this.document.body.appendChild(link);
    link.click();
    this.document.body.removeChild(link);
};

window.virtualScroll = {
    init: dotnetHelper => {
        window.onscroll = function () {
            var body = window.document.body;
            var html = window.document.documentElement;
            var scrollTop = body.scrollTop || html.scrollTop;
            var scrollBottom = html.scrollHeight - html.clientHeight - scrollTop;
            if (scrollTop != 0 && scrollBottom <= 2) { // 特定条件下でscrollHeightとclientHeightになる場合があるので、scrollTopもチェックする ※ディスプレイの種類によっては、一番下までスクロールした時のscrollBottomが0.3333の様にきっちり0にならないので余裕をもって2としている
                //一番下までスクロールした時に実行
                dotnetHelper.invokeMethodAsync('ScrollBottom');
            }
        };
    },
    normal: () => {
        window.onscroll = function () { };
    }
};

window.formSubmit = () => {
    var result = document.scform.checkValidity();
    if (!result) {
        document.getElementById("scsubmit").click();
    }
    return result;
}

window.navMenu = () => {
    document.getElementById("mainlayout_refresh").click();
}


var GLOBAL = {};
GLOBAL.DotNetReference = null;
GLOBAL.SetDotnetReference = function (pDotNetReference) {
    GLOBAL.DotNetReference = pDotNetReference;
};

windows.getPickingData = function () {
    console.log("test");
}

window.RemiseCreate = function (APPID, PASSWORD, inputData, Type) {
    console.log("remisecreate");
    var orTkn = new Remise.rTkn();
    orTkn.APPID = APPID;

    orTkn.PASSWORD = PASSWORD;
    orTkn.Card = inputData.cardNo;
    orTkn.Expire = inputData.cardExpire;
    orTkn.Cvc = inputData.cvc;
    orTkn.Name = inputData.cardOwner;
    orTkn.Type = Type;
    orTkn.Create();

    orTkn.OnSuccess = function () {
        console.log("Success");
        if (orTkn.Result == 0) {
            document.getElementById("TokenID").value = orTkn.TokenID;
            // カード番号、セキュリティコードをフォームから削除
            var element = document.getElementById("CardNo");
            element.parentNode.removeChild(element);
            element = document.getElementById("Cvc");
            element.parentNode.removeChild(element);
            GLOBAL.DotNetReference.invokeMethodAsync('RemiseTokenOnSuccess', orTkn.TokenID);
            return true;
        }
    }

    orTkn.OnError = function (errorcd) {
        console.log(errorcd);
        console.log("Error");
        GLOBAL.DotNetReference.invokeMethodAsync('RemiseTokenOnError', orTkn.Result);
    }
};

// used in `item_maps/new` page
function batch_import_form_check_ecsite(form_id) {
    var batch_import_form = document.getElementById(form_id);
    batch_import_form.reportValidity();
    return batch_import_form.checkValidity();
}

window.countryList = () => {
    return [
    {
        "code": "AD",
        "name": "アンドラ公国 (ANDORRA)"
    },
    {
        "code": "AE",
        "name": "アラブ首長国連邦 (UNITED ARAB EMIRATES)"
    },
    {
        "code": "AF",
        "name": "アフガニスタン・イスラム国 (AFGHANISTAN)"
    },
    {
        "code": "AG",
        "name": "アンチグア・バーブーダ (ANTIGUA AND BARBUDA)"
    },
    {
        "code": "AI",
        "name": "アンギラ (ANGUILLA)"
    },
    {
        "code": "AL",
        "name": "アルバニア共和国 (ALBANIA)"
    },
    {
        "code": "AM",
        "name": "アルメニア共和国 (ARMENIA)"
    },
    {
        "code": "AN",
        "name": "オランダ領アンティル (NETHERLANDS ANTILLES)"
    },
    {
        "code": "AO",
        "name": "アンゴラ共和国 (ANGOLA)"
    },
    {
        "code": "AQ",
        "name": "南極 (ANTARCTICA)"
    },
    {
        "code": "AR",
        "name": "アルゼンチン共和国 (ARGENTINA)"
    },
    {
        "code": "AS",
        "name": "米領サモア (AMERICAN SAMOA)"
    },
    {
        "code": "AT",
        "name": "オーストリア共和国 (AUSTRIA)"
    },
    {
        "code": "AU",
        "name": "オーストラリア (AUSTRALIA)"
    },
    {
        "code": "AW",
        "name": "アルバ (ARUBA)"
    },
    {
        "code": "AX",
        "name": "オーランド諸島 (ALAND ISLANDS)"
    },
    {
        "code": "AZ",
        "name": "アゼルバイジャン共和国 (AZERBAIJAN)"
    },
    {
        "code": "BA",
        "name": "ボスニア・ヘルツェゴビナ (BOSNIA AND HERZEGOVINA)"
    },
    {
        "code": "BB",
        "name": "バルバドス (BARBADOS)"
    },
    {
        "code": "BD",
        "name": "バングラデシュ人民共和国 (BANGLADESH)"
    },
    {
        "code": "BE",
        "name": "ベルギー王国 (BELGIUM)"
    },
    {
        "code": "BF",
        "name": "ブルキナファソ (BURKINA FASO)"
    },
    {
        "code": "BG",
        "name": "ブルガリア共和国 (BULGARIA)"
    },
    {
        "code": "BH",
        "name": "バーレーン王国 (BAHRAIN)"
    },
    {
        "code": "BI",
        "name": "ブルンジ共和国 (BURUNDI)"
    },
    {
        "code": "BJ",
        "name": "ベナン共和国 (BENIN)"
    },
    {
        "code": "BL",
        "name": "サン・バルテルミー島 (SAINT BARTHELEMY)"
    },
    {
        "code": "BM",
        "name": "バミューダ諸島 (BERMUDA)"
    },
    {
        "code": "BN",
        "name": "ブルネイ・ダルサラーム国 (BRUNEI DARUSSALAM)"
    },
    {
        "code": "BO",
        "name": "ボリビア多民族国 (BOLIVIA, PLURINATIONAL STATE OF)"
    },
    {
        "code": "BQ",
        "name": "英領南極地域 (BRITISH ANTARCTIC TERRITORY)"
    },
    {
        "code": "BR",
        "name": "ブラジル連邦共和国 (BRAZIL)"
    },
    {
        "code": "BS",
        "name": "バハマ国 (BAHAMAS)"
    },
    {
        "code": "BT",
        "name": "ブータン王国 (BHUTAN)"
    },
    {
        "code": "BU",
        "name": "ビルマ連邦社会主義共和国 (BURMA)"
    },
    {
        "code": "BV",
        "name": "ブーベ島 (BOUVET ISLAND)"
    },
    {
        "code": "BW",
        "name": "ボツワナ共和国 (BOTSWANA)"
    },
    {
        "code": "BY",
        "name": "ベラルーシ共和国 (BELARUS)"
    },
    {
        "code": "BZ",
        "name": "ベリーズ (BELIZE)"
    },
    {
        "code": "CA",
        "name": "カナダ (CANADA)"
    },
    {
        "code": "CC",
        "name": "ココス諸島 (COCOS (KEELING) ISLANDS)"
    },
    {
        "code": "CD",
        "name": "コンゴ民主共和国 (CONGO, DEMOCRATIC REPUBLIC OF THE)"
    },
    {
        "code": "CF",
        "name": "中央アフリカ共和国 (CENTRAL AFRICAN REPUBLIC)"
    },
    {
        "code": "CG",
        "name": "コンゴ共和国 (CONGO)"
    },
    {
        "code": "CH",
        "name": "スイス連邦 (SWITZERLAND)"
    },
    {
        "code": "CI",
        "name": "コートジボアール共和国 (COTE D'IVOIRE)"
    },
    {
        "code": "CK",
        "name": "クック諸島 (COOK ISLANDS)"
    },
    {
        "code": "CL",
        "name": "チリ共和国 (CHILE)"
    },
    {
        "code": "CM",
        "name": "カメルーン共和国 (CAMEROON)"
    },
    {
        "code": "CN",
        "name": "中華人民共和国 (CHINA)"
    },
    {
        "code": "CO",
        "name": "コロンビア共和国 (COLOMBIA)"
    },
    {
        "code": "CR",
        "name": "コスタリカ共和国 (COSTA RICA)"
    },
    {
        "code": "CS",
        "name": "セルビア・モンテネグロ (SERBIA AND MONTENEGRO)"
    },
    {
        "code": "CT",
        "name": "カントン島・エンダーバリ島 (CANTON AND ENDERBURY ISLANDS)"
    },
    {
        "code": "CU",
        "name": "キューバ共和国 (CUBA)"
    },
    {
        "code": "CV",
        "name": "カーボベルデ共和国 (CAPE VERDE)"
    },
    {
        "code": "CX",
        "name": "クリスマス島 (CHRISTMAS ISLAND)"
    },
    {
        "code": "CY",
        "name": "キプロス共和国 (CYPRUS)"
    },
    {
        "code": "CZ",
        "name": "チェコ共和国 (CZECH REPUBLIC)"
    },
    {
        "code": "DD",
        "name": "ドイツ民主共和国 (GERMAN DEMOCRATIC REPUBLIC)"
    },
    {
        "code": "DE",
        "name": "ドイツ連邦共和国 (GERMANY)"
    },
    {
        "code": "DJ",
        "name": "ジブチ共和国 (DJIBOUTI)"
    },
    {
        "code": "DK",
        "name": "デンマーク王国 (DENMARK)"
    },
    {
        "code": "DM",
        "name": "ドミニカ国 (DOMINICA)"
    },
    {
        "code": "DO",
        "name": "ドミニカ共和国 (DOMINICAN REPUBLIC)"
    },
    {
        "code": "DY",
        "name": "ダホメ共和国 (DAHOMEY)"
    },
    {
        "code": "DZ",
        "name": "アルジェリア民主人民共和国 (ALGERIA)"
    },
    {
        "code": "EC",
        "name": "エクアドル共和国 (ECUADOR)"
    },
    {
        "code": "EE",
        "name": "エストニア共和国 (ESTONIA)"
    },
    {
        "code": "EG",
        "name": "エジプト・アラブ共和国 (EGYPT)"
    },
    {
        "code": "EH",
        "name": "西サハラ (WESTERN SAHARA)"
    },
    {
        "code": "ER",
        "name": "エリトリア国 (ERITREA)"
    },
    {
        "code": "ES",
        "name": "スペイン (SPAIN)"
    },
    {
        "code": "ET",
        "name": "エチオピア連邦民主共和国 (ETHIOPIA)"
    },
    {
        "code": "FI",
        "name": "フィンランド共和国 (FINLAND)"
    },
    {
        "code": "FJ",
        "name": "フィジー諸島共和国 (FIJI)"
    },
    {
        "code": "FK",
        "name": "フォークランド(マルビナス)諸島 (FALKLAND ISLANDS(MALVINAS))"
    },
    {
        "code": "FM",
        "name": "ミクロネシア連邦 (MICRONESIA, FEDERATED STATES OF)"
    },
    {
        "code": "FO",
        "name": "フェロー諸島 (FAROE ISLANDS)"
    },
    {
        "code": "FQ",
        "name": "仏領諸島 (FRENCH SOUTHERN AND ANTARCTIC TERRITORIES)"
    },
    {
        "code": "FR",
        "name": "フランス共和国 (FRANCE)"
    },
    {
        "code": "FX",
        "name": "フランス本国 (FRANCE, METROPOLITAN)"
    },
    {
        "code": "GA",
        "name": "ガボン共和国 (GABON)"
    },
    {
        "code": "GB",
        "name": "グレートブリテンおよび北部アイルランド連合王国(英国) (UNITED KINGDOM)"
    },
    {
        "code": "GD",
        "name": "グレナダ (GRENADA)"
    },
    {
        "code": "GE",
        "name": "ジョージア (GEORGIA)"
    },
    {
        "code": "GF",
        "name": "フランス領ギアナ (FRENCH GUIANA)"
    },
    {
        "code": "GG",
        "name": "ガーンジー島 (GUERNSEY)"
    },
    {
        "code": "GH",
        "name": "ガーナ共和国 (GHANA)"
    },
    {
        "code": "GI",
        "name": "ジブラルタル (GIBRALTAR)"
    },
    {
        "code": "GL",
        "name": "グリーンランド (GREENLAND)"
    },
    {
        "code": "GM",
        "name": "ガンビア共和国 (GAMBIA)"
    },
    {
        "code": "GN",
        "name": "ギニア共和国 (GUINEA)"
    },
    {
        "code": "GP",
        "name": "グアドループ島 (GUADELOUPE)"
    },
    {
        "code": "GQ",
        "name": "赤道ギニア共和国 (EQUATORIAL GUINEA)"
    },
    {
        "code": "GR",
        "name": "ギリシア共和国 (GREECE)"
    },
    {
        "code": "GS",
        "name": "南ジョージア島・南サンドイッチ諸島 (SOUTH GEORGIA AND THE SOUTH SANDWICH ISLANDS)"
    },
    {
        "code": "GT",
        "name": "グアテマラ共和国 (GUATEMALA)"
    },
    {
        "code": "GU",
        "name": "グアム (GUAM)"
    },
    {
        "code": "GW",
        "name": "ギニアビサウ共和国 (GUINEA-BISSAU)"
    },
    {
        "code": "GY",
        "name": "ガイアナ協同共和国 (GUYANA)"
    },
    {
        "code": "HK",
        "name": "香港特別行政区(ホンコン) (HONG KONG)"
    },
    {
        "code": "HM",
        "name": "ヘアド島・マクドナルド諸島 (HEARD ISLAND AND MCDONALD ISLANDS)"
    },
    {
        "code": "HN",
        "name": "ホンジュラス共和国 (HONDURAS)"
    },
    {
        "code": "HR",
        "name": "クロアチア共和国 (CROATIA)"
    },
    {
        "code": "HT",
        "name": "ハイチ共和国 (HAITI)"
    },
    {
        "code": "HU",
        "name": "ハンガリー共和国 (HUNGARY)"
    },
    {
        "code": "HV",
        "name": "オートボルタ共和国 (UPPER VOLTA)"
    },
    {
        "code": "ID",
        "name": "インドネシア共和国 (INDONESIA)"
    },
    {
        "code": "IE",
        "name": "アイルランド (IRELAND)"
    },
    {
        "code": "IL",
        "name": "イスラエル国 (ISRAEL)"
    },
    {
        "code": "IM",
        "name": "マン島 (ISLE OF MAN)"
    },
    {
        "code": "IN",
        "name": "インド (INDIA)"
    },
    {
        "code": "IO",
        "name": "英領インド洋地域 (BRITISH INDIAN OCEAN TERRITORY)"
    },
    {
        "code": "IQ",
        "name": "イラク共和国 (IRAQ)"
    },
    {
        "code": "IR",
        "name": "イラン・イスラム共和国 (IRAN, ISLAMIC REPUBLIC OF)"
    },
    {
        "code": "IS",
        "name": "アイスランド共和国 (ICELAND)"
    },
    {
        "code": "IT",
        "name": "イタリア共和国 (ITALY)"
    },
    {
        "code": "JE",
        "name": "ジャージー島 (JERSEY)"
    },
    {
        "code": "JM",
        "name": "ジャマイカ (JAMAICA)"
    },
    {
        "code": "JO",
        "name": "ヨルダン・ハシミテ王国 (JORDAN)"
    },
    {
        "code": "JP",
        "name": "日本国 (JAPAN)"
    },
    {
        "code": "JT",
        "name": "ジョンストン島 (JOHNSTON ISLAND)"
    },
    {
        "code": "KE",
        "name": "ケニア共和国 (KENYA)"
    },
    {
        "code": "KG",
        "name": "キルギス共和国 (KYRGYZSTAN)"
    },
    {
        "code": "KH",
        "name": "カンボジア王国 (CAMBODIA)"
    },
    {
        "code": "KI",
        "name": "キリバス共和国 (KIRIBATI)"
    },
    {
        "code": "KM",
        "name": "コモロ連合 (COMOROS)"
    },
    {
        "code": "KN",
        "name": "セントクリストファー・ネイビス (SAINT KITTS AND NEVIS)"
    },
    {
        "code": "KP",
        "name": "(北朝鮮=朝鮮民主主義人民共和国) (KOREA, DEMOCRATIC PEOPLE'S REPUBLIC OF)"
    },
    {
        "code": "KR",
        "name": "大韓民国 (KOREA, REPUBLIC OF)"
    },
    {
        "code": "KW",
        "name": "クウェート国 (KUWAIT)"
    },
    {
        "code": "KY",
        "name": "ケイマン諸島 (CAYMAN ISLANDS)"
    },
    {
        "code": "KZ",
        "name": "カザフスタン共和国 (KAZAKHSTAN)"
    },
    {
        "code": "LA",
        "name": "ラオス人民民主共和国 (LAO PEOPLE'S DEMOCRATIC REPUBLIC)"
    },
    {
        "code": "LB",
        "name": "レバノン共和国 (LEBANON)"
    },
    {
        "code": "LC",
        "name": "セントルシア (SAINT LUCIA)"
    },
    {
        "code": "LI",
        "name": "リヒテンシュタイン公国 (LIECHTENSTEIN)"
    },
    {
        "code": "LK",
        "name": "スリランカ民主社会主義共和国 (SRI LANKA)"
    },
    {
        "code": "LR",
        "name": "リベリア共和国 (LIBERIA)"
    },
    {
        "code": "LS",
        "name": "レソト王国 (LESOTHO)"
    },
    {
        "code": "LT",
        "name": "リトアニア共和国 (LITHUANIA)"
    },
    {
        "code": "LU",
        "name": "ルクセンブルク大公国 (LUXEMBOURG)"
    },
    {
        "code": "LV",
        "name": "ラトビア共和国 (LATVIA)"
    },
    {
        "code": "LY",
        "name": "社会主義人民リビア・アラブ国 (LIBYAN ARAB JAMAHIRIYA)"
    },
    {
        "code": "MA",
        "name": "モロッコ王国 (MOROCCO)"
    },
    {
        "code": "MC",
        "name": "モナコ公国 (MONACO)"
    },
    {
        "code": "MD",
        "name": "モルドバ共和国 (MOLDOVA, REPUBLIC OF)"
    },
    {
        "code": "ME",
        "name": "モンテネグロ (MONTENEGRO)"
    },
    {
        "code": "MF",
        "name": "サン・マルタン島 (SAINT MARTIN (FRENCH PART))"
    },
    {
        "code": "MG",
        "name": "マダガスカル共和国 (MADAGASCAR)"
    },
    {
        "code": "MH",
        "name": "マーシャル諸島共和国 (MARSHALL ISLANDS)"
    },
    {
        "code": "MI",
        "name": "ミッドウェー諸島 (MIDWAY ISLANDS)"
    },
    {
        "code": "MK",
        "name": "マケドニア旧ユーゴスラビア共和国 (MACEDONIA, THE FORMER YUGOSLAV REPUBLIC OF)"
    },
    {
        "code": "ML",
        "name": "マリ共和国 (MALI)"
    },
    {
        "code": "MM",
        "name": "ミャンマー連邦 (MYANMAR)"
    },
    {
        "code": "MN",
        "name": "モンゴル国 (MONGOLIA)"
    },
    {
        "code": "MO",
        "name": "マカオ(澳門) (MACAO)"
    },
    {
        "code": "MP",
        "name": "北マリアナ諸島 (NORTHERN MARIANA ISLANDS)"
    },
    {
        "code": "MQ",
        "name": "マルチニーク島 (MARTINIQUE)"
    },
    {
        "code": "MR",
        "name": "モーリタニア・イスラム共和国 (MAURITANIA)"
    },
    {
        "code": "MS",
        "name": "モンセラット (MONTSERRAT)"
    },
    {
        "code": "MT",
        "name": "マルタ共和国 (MALTA)"
    },
    {
        "code": "MU",
        "name": "モーリシャス共和国 (MAURITIUS)"
    },
    {
        "code": "MV",
        "name": "モルディヴ共和国 (MALDIVES)"
    },
    {
        "code": "MW",
        "name": "マラウイ共和国 (MALAWI)"
    },
    {
        "code": "MX",
        "name": "メキシコ合衆国 (MEXICO)"
    },
    {
        "code": "MY",
        "name": "マレーシア (MALAYSIA)"
    },
    {
        "code": "MZ",
        "name": "モザンビーク共和国 (MOZAMBIQUE)"
    },
    {
        "code": "NA",
        "name": "ナミビア共和国 (NAMIBIA)"
    },
    {
        "code": "NC",
        "name": "ニューカレドニア (NEW CALEDONIA)"
    },
    {
        "code": "NE",
        "name": "ニジェール共和国 (NIGER)"
    },
    {
        "code": "NF",
        "name": "ノーフォーク島 (NORFOLK ISLAND)"
    },
    {
        "code": "NG",
        "name": "ナイジェリア連邦共和国 (NIGERIA)"
    },
    {
        "code": "NH",
        "name": "ニューヘブリデス (NEW HEBRIDES)"
    },
    {
        "code": "NI",
        "name": "ニカラグア共和国 (NICARAGUA)"
    },
    {
        "code": "NL",
        "name": "オランダ王国 (NETHERLANDS)"
    },
    {
        "code": "NO",
        "name": "ノルウェー王国 (NORWAY)"
    },
    {
        "code": "NP",
        "name": "ネパール連邦民主共和国 (NEPAL)"
    },
    {
        "code": "NQ",
        "name": "ドロニング・マウド (DRONNING MAUD LAND)"
    },
    {
        "code": "NR",
        "name": "ナウル共和国 (NAURU)"
    },
    {
        "code": "NT",
        "name": "中立地帯 (NEUTRAL ZONE)"
    },
    {
        "code": "NU",
        "name": "ニウエ (NIUE)"
    },
    {
        "code": "NZ",
        "name": "ニュージーランド (NEW ZEALAND)"
    },
    {
        "code": "OM",
        "name": "オマーン国 (OMAN)"
    },
    {
        "code": "PA",
        "name": "パナマ共和国 (PANAMA)"
    },
    {
        "code": "PC",
        "name": "太平洋諸島信託統治地域 (PACIFIC ISLANDS(trust territory))"
    },
    {
        "code": "PE",
        "name": "ペルー共和国 (PERU)"
    },
    {
        "code": "PF",
        "name": "フランス領ポリネシア (FRENCH POLYNESIA)"
    },
    {
        "code": "PG",
        "name": "パプアニューギニア (PAPUA NEW GUINEA)"
    },
    {
        "code": "PH",
        "name": "フィリピン共和国 (PHILIPPINES)"
    },
    {
        "code": "PK",
        "name": "パキスタン・イスラム共和国 (PAKISTAN)"
    },
    {
        "code": "PL",
        "name": "ポーランド共和国 (POLAND)"
    },
    {
        "code": "PM",
        "name": "サンピエール島・ミクロン島 (SAINT PIERRE AND MIQUELON)"
    },
    {
        "code": "PN",
        "name": "ピトケアン島 (PITCAIRN)"
    },
    {
        "code": "PR",
        "name": "プエルトリコ (PUERTO RICO)"
    },
    {
        "code": "PS",
        "name": "パレスチナ占領区域 (PALESTINIAN TERRITORY, OCCUPIED)"
    },
    {
        "code": "PT",
        "name": "ポルトガル共和国 (PORTUGAL)"
    },
    {
        "code": "PU",
        "name": "米領太平洋諸島 (UNITED STATES MISC. PACIFIC ISLANDS)"
    },
    {
        "code": "PW",
        "name": "パラオ共和国 (PALAU)"
    },
    {
        "code": "PY",
        "name": "パラグアイ共和国 (PARAGUAY)"
    },
    {
        "code": "QA",
        "name": "カタール国 (QATAR)"
    },
    {
        "code": "RE",
        "name": "レユニオン (REUNION)"
    },
    {
        "code": "RH",
        "name": "南ローデシア (SOUTHERN RHODESIA)"
    },
    {
        "code": "RO",
        "name": "ルーマニア (ROMANIA)"
    },
    {
        "code": "RS",
        "name": "セルビア共和国 (SERBIA)"
    },
    {
        "code": "RU",
        "name": "ロシア連邦 (RUSSIAN FEDERATION)"
    },
    {
        "code": "RW",
        "name": "ルワンダ共和国 (RWANDA)"
    },
    {
        "code": "SA",
        "name": "サウジアラビア王国 (SAUDI ARABIA)"
    },
    {
        "code": "SB",
        "name": "ソロモン諸島 (SOLOMON ISLANDS)"
    },
    {
        "code": "SC",
        "name": "セイシェル共和国 (SEYCHELLES)"
    },
    {
        "code": "SD",
        "name": "スーダン共和国 (SUDAN)"
    },
    {
        "code": "SE",
        "name": "スウェーデン王国 (SWEDEN)"
    },
    {
        "code": "SG",
        "name": "シンガポール共和国 (SINGAPORE)"
    },
    {
        "code": "SH",
        "name": "セントヘレナ島 (SAINT HELENA)"
    },
    {
        "code": "SI",
        "name": "スロベニア共和国 (SLOVENIA)"
    },
    {
        "code": "SJ",
        "name": "スバールバル諸島・ヤンマイエン島 (SVALBARD AND JAN MAYEN)"
    },
    {
        "code": "SK",
        "name": "スロバキア共和国 (SLOVAKIA)"
    },
    {
        "code": "SL",
        "name": "シエラレオネ共和国 (SIERRA LEONE)"
    },
    {
        "code": "SM",
        "name": "サンマリノ共和国 (SAN MARINO)"
    },
    {
        "code": "SN",
        "name": "セネガル共和国 (SENEGAL)"
    },
    {
        "code": "SO",
        "name": "ソマリア民主共和国 (SOMALIA)"
    },
    {
        "code": "SR",
        "name": "スリナム共和国 (SURINAME)"
    },
    {
        "code": "ST",
        "name": "サントメ・プリンシペ民主共和国 (SAO TOME AND PRINCIPE)"
    },
    {
        "code": "SU",
        "name": "ソビエト社会主義共和国連邦 (USSR)"
    },
    {
        "code": "SV",
        "name": "エルサルバドル共和国 (EL SALVADOR)"
    },
    {
        "code": "SY",
        "name": "シリア・アラブ共和国 (SYRIAN ARAB REPUBLIC)"
    },
    {
        "code": "SZ",
        "name": "スワジランド王国 (SWAZILAND)"
    },
    {
        "code": "TC",
        "name": "タークス諸島・カイコス諸島 (TURKS AND CAICOS ISLANDS)"
    },
    {
        "code": "TD",
        "name": "チャド共和国 (CHAD)"
    },
    {
        "code": "TF",
        "name": "フランス領極南諸島 (FRENCH SOUTHERN TERRITORIES)"
    },
    {
        "code": "TG",
        "name": "トーゴ共和国 (TOGO)"
    },
    {
        "code": "TH",
        "name": "タイ王国 (THAILAND)"
    },
    {
        "code": "TJ",
        "name": "タジキスタン共和国 (TAJIKISTAN)"
    },
    {
        "code": "TK",
        "name": "トケラウ諸島 (TOKELAU)"
    },
    {
        "code": "TL",
        "name": "東ティモール民主共和国 (TIMOR-LESTE)"
    },
    {
        "code": "TM",
        "name": "トルクメニスタン (TURKMENISTAN)"
    },
    {
        "code": "TN",
        "name": "チュニジア共和国 (TUNISIA)"
    },
    {
        "code": "TO",
        "name": "トンガ王国 (TONGA)"
    },
    {
        "code": "TP",
        "name": "東ティモール (EAST TIMOR)"
    },
    {
        "code": "TR",
        "name": "トルコ共和国 (TURKEY)"
    },
    {
        "code": "TT",
        "name": "トリニダード・トバゴ共和国 (TRINIDAD AND TOBAGO)"
    },
    {
        "code": "TV",
        "name": "ツバル (TUVALU)"
    },
    {
        "code": "TW",
        "name": "タイワン(台湾) (TAIWAN, PROVINCE OF CHINA)"
    },
    {
        "code": "TZ",
        "name": "タンザニア連合共和国 (TANZANIA, UNITED REPUBLIC OF)"
    },
    {
        "code": "UA",
        "name": "ウクライナ (UKRAINE)"
    },
    {
        "code": "UG",
        "name": "ウガンダ共和国 (UGANDA)"
    },
    {
        "code": "UM",
        "name": "米領太平洋諸島 (UNITED STATES MINOR OUTLYING ISLANDS)"
    },
    {
        "code": "US",
        "name": "アメリカ合衆国(米国) (UNITED STATES)"
    },
    {
        "code": "UY",
        "name": "ウルグアイ東方共和国 (URUGUAY)"
    },
    {
        "code": "UZ",
        "name": "ウズベキスタン共和国 (UZBEKISTAN)"
    },
    {
        "code": "VA",
        "name": "バチカン市国 (HOLY SEE (VATICAN CITY STATE))"
    },
    {
        "code": "VC",
        "name": "セントビンセントおよびグレナディーン諸島 (SAINT VINCENT AND THE GRENADINES)"
    },
    {
        "code": "VD",
        "name": "ベトナム民主共和国 (VIETNAM, DEMOCRATIC REPUBLIC OF)"
    },
    {
        "code": "VE",
        "name": "ベネズエラ・ボリバル共和国 (VENEZUELA, BOLIVARIAN REPUBLIC OF)"
    },
    {
        "code": "VG",
        "name": "英領バージン諸島 (VIRGIN ISLANDS, BRITISH)"
    },
    {
        "code": "VI",
        "name": "米領バージン諸島 (VIRGIN ISLANDS, U.S.)"
    },
    {
        "code": "VN",
        "name": "ベトナム社会主義共和国 (VIET NAM)"
    },
    {
        "code": "VU",
        "name": "バヌアツ共和国 (VANUATU)"
    },
    {
        "code": "WF",
        "name": "ワリス・フテュナ諸島 (WALLIS AND FUTUNA)"
    },
    {
        "code": "WK",
        "name": "ウェーク島 (WAKE ISLAND)"
    },
    {
        "code": "WS",
        "name": "サモア独立国 (SAMOA)"
    },
    {
        "code": "XX",
        "name": "出版地不明 (NO PLACE, UNKNOWN, OR UNDETERMINED)"
    },
    {
        "code": "YD",
        "name": "イエメン民主人民共和国 (YEMEN, DEMOCRATIC)"
    },
    {
        "code": "YE",
        "name": "イエメン共和国 (YEMEN)"
    },
    {
        "code": "YT",
        "name": "マイヨット島 (MAYOTTE)"
    },
    {
        "code": "YU",
        "name": "ユーゴスラビア連邦共和国 (YUGOSLAVIA)"
    },
    {
        "code": "ZA",
        "name": "南アフリカ共和国 (SOUTH AFRICA)"
    },
    {
        "code": "ZM",
        "name": "ザンビア共和国 (ZAMBIA)"
    },
    {
        "code": "ZR",
        "name": "ザイール共和国 (ZAIRE)"
    },
    {
        "code": "ZW",
        "name": "ジンバブエ共和国 (ZIMBABWE)"
    }
]};

// select form val
function setSelectValue(id, v){
    document.getElementById(id).value = v
}