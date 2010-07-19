# -*- coding: utf-8 -*-

import os
import logging
import random
from google.appengine.api import channel, users
from google.appengine.ext import webapp, db
from google.appengine.ext.webapp.util import run_wsgi_app
from google.appengine.ext.webapp import template
from django.utils import simplejson
from string import Template

#プレイヤーデータ
class PlayerData(db.Model):
    name = db.StringProperty();
    color = db.IntegerProperty();
    createTime = db.DateTimeProperty(auto_now=True)
 
#エリアデータ
class AreaData(db.Model):
    number = db.IntegerProperty()
    players = db.ListProperty(item_type=db.Key, default=[])
    colors = db.ListProperty(item_type=int, default=[])

#マップデータ
class MapData(db.Model):
    areaDatas = db.ListProperty(item_type=db.Key, default=[])
 
#試合データ
class MatchData(db.Model):
    id = db.IntegerProperty();
    players = db.ListProperty(item_type=db.Key, default=[])
    playerNum = db.IntegerProperty(default=0)
    mapSeed = db.IntegerProperty(default=0)
    createTime = db.DateTimeProperty(auto_now=True)

#アプリデータ
class ApplicationData(db.Model):
    matchCount = db.IntegerProperty(default=0);


#メインページ
class MainPage(webapp.RequestHandler):
    def get(self):
        logging.log(25, 'MainPage GET:' )

        slightParamTemp = Template('<param name="initParams" value="$value"/>')
        slightValue = ''
        slightValue += 'hellow=Hellow NicoNico!!'
        slightValue += ',count=5'
        slightParam = slightParamTemp.substitute(value=slightValue)

        path = os.path.join(os.path.dirname(__file__), 'slight', 'TestPage.html')
        logging.log(25, 'fpath:' + path)
        html = template.render(path, {'slightParam':slightParam})
        self.response.out.write(html)

#コマンドページ
class CommandPage(webapp.RequestHandler):

    def get(self):
        self.post()
        
    def post(self):
        commandTable = dict()
        commandTable['polling'] = self.Polling
        commandTable['entry'] = self.Entry
        commandTable['area_click'] = self.AreaClick

        data = simplejson.loads(self.request.get('data'))
        command = data['command']

        if command in commandTable:
            messageData = commandTable[command](data)
        else:
            messageData = "not impl command :" + command

        message = simplejson.dumps(messageData, ensure_ascii=False)
        logging.log(25, "command return:" + message)
        self.response.out.write(message)

    def Entry(self, data):
        mapSeed = random.randint(0,0x7FFFFFFF)
        appData = ApplicationData.get_or_insert('app_data', mapSeed=mapSeed)
        matchId = appData.matchCount

        logging.log(25, 'match ID :'  + str(appData.matchCount))
        
        matchKeyName = 'match_' + str(matchId)
        matchData = MatchData.get_or_insert(key_name=matchKeyName, id=matchId)

        logging.log(25, 'match key :'  + matchKeyName)
        logging.log(25, 'matchData playerNum:'  + str(matchData.playerNum))

        if (matchData.playerNum >= 4):
            logging.log(25, '---- create new match ----')
            appData.matchCount += 1
            appData.put()

            logging.log(25, 'match ID :'  + str(appData.matchCount))
            matchId = appData.matchCount
            matchKeyName = 'match_' + str(matchId)
            matchData = MatchData.get_or_insert(key_name=matchKeyName, id=matchId, mapSeed=mapSeed)

            logging.log(25, 'match key :'  + matchKeyName)

        playerNumber = matchData.playerNum
        color = [0xFFFF0000, 0xFF00FF00, 0xFF0000FF, 0xFFFFFF88][playerNumber]
        name = ['p1', 'p2', 'p3', 'p4'][playerNumber]
        playerKeyName = 'player_' + str(matchId) + '_' + str(playerNumber)
        playerData = PlayerData.get_or_insert(key_name = playerKeyName, color=color, name=name)

        matchData.players.append(playerData.key())
        matchData.playerNum += 1
        matchData.put()

        logging.log(25, 'create player :'  + str(playerNumber))
        logging.log(25, '        key   :'  + playerKeyName)
        logging.log(25, '        name  :'  + playerData.name)
        logging.log(25, '        color :'  + str(playerData.color))

        playerInfo = dict()
        playerInfo['keyName'] = playerKeyName
        playerInfo['name'] = playerData.name
        playerInfo['color'] = playerData.color

        matchInfo = dict()
        matchInfo['keyName'] = matchKeyName
        matchInfo['mapSeed'] = matchData.mapSeed

        message = dict()
        message['command'] = 'entry'
        message['matchInfo'] = matchInfo
        message['playerInfo'] = playerInfo

        return message

    def Polling(self, data):
        matchKeyName = data['matchKeyName']
        mapKeyName = matchKeyName + "_map"
        mapData = MapData.get_or_insert(mapKeyName)

        message = dict()
        message['command'] = 'areaData'
        message['areaDatas'] = self.GetAreaList(mapData.areaDatas)

        return message

    def AreaClick(self, data):
        matchKeyName = data['matchKeyName']
        playerKeyName = data['playerKeyName']     
        mapKeyName = matchKeyName + "_map"
        areaKeyName = matchKeyName + "_area_" + str(data['areaNumber'])

        areaNumber = int(data['areaNumber'])

        playerData = PlayerData.get_by_key_name(playerKeyName)
        mapData = MapData.get_or_insert(mapKeyName)
        areaData = AreaData.get_or_insert(key_name=areaKeyName, number=areaNumber)

        try:
            mapData.areaDatas.index(areaData.key())
        except ValueError:
            mapData.areaDatas.append(areaData.key())
            mapData.put();

        try:        
            areaData.players.index(playerData.key())
        except ValueError:
            areaData.players.append(playerData.key())
            areaData.colors.append(playerData.color)
            areaData.put();

            logging.log(25, 'click area   :'  + areaKeyName)
            logging.log(25, '      player :'  + playerKeyName)
            logging.log(25, '      color  :'  + str(playerData.color))

        message = dict()
        message['command'] = 'areaData'
        message['areaDatas'] = self.GetAreaList(mapData.areaDatas)

        return message

    def GetAreaList(self, areaDatas):
        areaList = []
        for areaKey in areaDatas:
            area = AreaData.get(areaKey)

            players = []
            for playerKey in area.players:
                player = PlayerData.get(playerKey)
                players.append(player.key().name())

            data = dict()
            data['number'] = area.number
            data['playres'] = players
            data['colors'] = area.colors
            areaList.append(data)
        
        return areaList
            

application = webapp.WSGIApplication([('/', MainPage),
                                      ('/command', CommandPage),
 #                                     ('/polling', PollingPage),
                                     ],
                                      debug=True)


def main():
    run_wsgi_app(application)

if __name__ == "__main__":
    main()
