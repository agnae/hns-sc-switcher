# hns-sc-switcher
![hns-sc-switcher logo](https://user-images.githubusercontent.com/4912057/198945190-f7591003-b27b-40bf-9fcd-e07419b03e2a.png)

Algo switcher between blake2bsha3(HNS) &amp; blake2b(SC) for mining hardware by Goldshell

## Confirmed with the following hardware/firmware
| Model      | 2.2.1 | 2.2.2 | 2.2.3 |
| ---------- | :---: | :---: | :---: |
| HSBox      | ✅    |   ❔  | ✅    |
| HSLITE     |   ❔  | ✅    |  ❔   |

## Usage
Update `appsettings.config` with your miner's IP address and pool information for each miner & hashing algo

`switchLag` is the delay (in ms) between switching consecutive miners

### Execute
  `hns-sc-switcher` will read out current hashrates
  
  `hns-sc-switcher hns` will switch a miner to mine HNS if it is mining SC
  
  `hns-sc-switcher sc` will switch a miner to mine SC if it is mining HNS
  
### Other usages
Crond example (switch on a schedule)
```
0  19 *  *  *  /usr/local/bin/hs-sia-switcher SC
0  3  *  *  *  /usr/local/bin/hs-sia-switcher HNS
```

Send email alert (with mailgun)
```
0  19 *  *  *  /usr/local/bin/hs-sia-switcher SC 2>&1 | curl -s --user 'api:YOUR_API_KEY' https://api.mailgun.net/v3/YOUR_DOMAIN_NAME/messages -F from='YOUR_FROM_EMAIL' -F to='YOUR_TO_EMAIL' -F subject='HNS-SC Switcher | Switched to SC' -F text="$(xargs -0)"
0  3  *  *  *  /usr/local/bin/hs-sia-switcher HNS  2>&1 | curl -s ---user 'api:YOUR_API_KEY' https://api.mailgun.net/v3/YOUR_DOMAIN_NAME/messages -F from='YOUR_FROM_EMAIL' -F to='YOUR_TO_EMAIL' -F subject='HNS-SC Switcher | Switched to HNS' -F text="$(xargs -0)"
3  3,19  *  *  *  /usr/local/bin/hs-sia-switcher 2>&1 | curl -s --user 'api:YOUR_API_KEY' https://api.mailgun.net/v3/YOUR_DOMAIN_NAME/messages -F from='YOUR_FROM_EMAIL' -F to='YOUR_TO_EMAIL' -F subject='HNS-SC Switcher | Stats' -F text="$(xargs -0)"
```
