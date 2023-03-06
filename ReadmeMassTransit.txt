From: 011netservice@gmail.com
Date: 2023-03-04
Subject: MassTransit with RabbitMQ
File: https://github.com/github-honda/MassTransitPratice/blob/main/ReadmeMassTransit.txt

歡迎來信交流, 訂購軟體需求.

MassTransit:
  https://masstransit.io/
  https://masstransit-project.com/
  https://github.com/MassTransit/MassTransit

  2016-08 版本 .Net Framework 的範例:
  https://dotnetcodr.com/messaging/
    原始碼: 
	https://github.com/github-honda/MassTransitPratice/tree/main/Net48/2016Lab/V2016  
  
RabbitMQ: 
  https://www.rabbitmq.com/
  
  
#### .NETCore 範例
初學者先了解 (In Memory) 和 (RabbitMQ) 2個 Getting Started, 執行環境必須為 .NET 6.0 以上.

□ GettingStarted-InMemory 
https://masstransit.io/quick-starts/in-memory
Getting Started - In Memory.pdf
In-Memory 以記憶體傳輸模式特性:
◇ 不需要依賴訊息代理元件(例如 RabbitMQ).
◇ 只能在本機運作.
◇ 若 bus 停止後, 訊息就消失了, 不會記憶.
◇ 不支援多執行緒, 只適用於單工執行環境.
The in-memory transport is a great tool for testing, as it doesn't require a message broker to be installed or running. 
It's also very fast. 
But it isn't durable, and messages are gone if the bus is stopped or the process terminates. 
So, it's generally not a smart option for a production system. 
However, there are places where durability is not important so the cautionary tale is to proceed with caution.
WARNING
The in-memory transport is intended for use within a single process only. It cannot be used to communicate between multiple processes (even if they are on the same machine).

建立步驟:
○ 確認已安裝至少 SDK 6.0
$ dotnet --list-sdks
6.0.405 [C:\Program Files\dotnet\sdk]
7.0.102 [C:\Program Files\dotnet\sdk]

○ 安裝 MassTransit.Templates
Install MassTransit Templates
$ dotnet new --install MassTransit.Templates
成功: MassTransit.Templates::1.0.6 已安裝下列範本...

○ 建立專案 
Create the project
$ dotnet new mtworker -n GettingStarted
$ cd GettingStarted
$ dotnet new mtconsumer
原始碼可參考官網上的步驟產生, 或是從筆者的網站上取得:
https://github.com/github-honda/MassTransitPratice/tree/main/NET60/GettingStarted

舊版原始碼: 官網改到 https://masstransit.io/ 之前的原始碼也可以用.
https://github.com/github-honda/MassTransitPratice/tree/main/GettingStarted-InMemory

○ 執行程式
$ dotnet run
 
□  Getting Started - RabbitMQ
https://masstransit.io/quick-starts/rabbitmq

建立步驟:
○ 確認 GettingStarted-InMemory 方式, 已可正確執行.

○ Run RabbitMQ docker
官網: $ docker run -p 15672:15672 -p 5672:5672 masstransit/rabbitmq
建議改成:
$ docker run -p 15672:15672 -p 5672:5672 --name masstransit1 masstransit/rabbitmq 
  -p, --publish list                   Publish a container's port(s) to
                                       the host
15672 is the default port for RabbitMQ GUI, 
5672 for RabbitMQ message broker.

因為若未加參數 --name, 則每次執行就會新增一個新的(自動命名)的 container image.

若是 ARM platform, 則指令為 $ docker run --platform linux/arm64 -p 15672:15672 -p 5672:5672 masstransit/rabbitmq

○ 測試 RabbitMQ docker 管理介面
瀏覽網頁到 http://localhost:15672 測試登入.
登入帳號為 guest, 密碼guest, 這組預設帳密, 只能在本機 localhost 使用.
15672 is the default port for RabbitMQ GUI, 
5672 for RabbitMQ message broker.
能看到網頁就好, 不需要登入也可以.

或是用指令測試: $ docker exec [DockerImageName] rabbitmqctl status | 檢查 RabbitMQ Docker Image 的狀態.
例如:
$ docker exec MassTransit1 rabbitmqctl status


○ 在原專案中安裝 MassTransit.RabbitMQ package.
$ dotnet add package MassTransit.RabbitMQ
或是直接下載筆者的專案: https://github.com/github-honda/MassTransitPratice/tree/main/NET60/RabbitMQ

○ 原專案原始碼修改
可參考官網上的步驟修改, 或是從筆者的網站上取得:
https://github.com/github-honda/MassTransitPratice/tree/main/NET60/RabbitMQ

舊版原始碼: 官網改到 https://masstransit.io/ 之前的原始碼也可以用.
https://github.com/github-honda/MassTransitPratice/tree/main/GettingStarted-RabbitMq

○ 執行程式
$ dotnet run


#### 2016-08 RabbitMQ.NET Updated series  
https://dotnetcodr.com/messaging/ 

原始碼: 
https://github.com/github-honda/MassTransitPratice/tree/main/Net48/2016Lab/V2016  

□ Part 1. 基本概念
Messaging with RabbitMQ and .NET review part 1: foundations and terminology
https://dotnetcodr.com/2016/08/02/messaging-with-rabbitmq-and-net-review-part-1-foundations-and-terminology/

□ Part 2. 安裝設定 RabbitMQ
Messaging with RabbitMQ and .NET review part 2: installation and setup
https://dotnetcodr.com/2016/08/03/messaging-with-rabbitmq-and-net-review-part-2-installation-and-setup/
原範例是安裝 RabbitMQ 到主 Host, 再設定一些配合範例程式執行的 RabbitMQ 參數.
筆者改成安裝為 RabbitMQ Docker後, 在設定同樣的參數如下:
○ 安裝 RabbitMQ Docker
請先參考( #### 安裝 MassTransit1 RabbitMQ Docker), 安裝 RabbitMQ Docker, 不要安裝 RabbitMQ 到主 Host.

○ 設定 RabbitMQ 參數
安裝 RabbitMQ Docker 後, 啟動執行.
再以瀏覽器連線到 http://localhost:15672, 以帳號 guest, 密碼 guest 登入.

△ 設定 Virtual Hosts
在 RabbitMQ 管理介面網頁上, 選擇 Admin.Virtual Hosts.Add virtual host 如下:
Name: accounting
Description: 空白
Tags: 空白

△ Add Users
在 RabbitMQ 管理介面網頁上, 選擇 Admin.Users.Add Users 如下:
Username: accountant
Password: accountant
Confirm:  accountant
Tags: administrator (若非 predefined roles, 則無法登入管理介面).


△ 授權 Virtual Host 給 User
在 RabbitMQ 管理介面網頁上, 選擇 Admin.Users, 在使用者清單中選擇使用者, 例如 accountant.
在 Set Permission 區段中, 填入:
Virtual Host: 選擇 accounting 
Configure regexp: .*
Write regexp: .*
Read regexp: .*
按下 Set Permission 按鍵後, 可看到新增授權在上方 Current permissions 清單中.

□ Part 3. 以程式測試發佈訊息
Messaging with RabbitMQ and .NET review part 3: the .NET client and some initial code
https://dotnetcodr.com/2016/08/05/messaging-with-rabbitmq-and-net-review-part-3-the-net-client-and-some-initial-code/

示範 Open Channel 和 Publishing message.
1. 偵測可否連線.
2. 建立 RabbitMQ Channel.
2.1 Durable: Message 會持續保留在 RabbitMQ 中, 即使關機後重新開啟, 仍會存留訊息.
3. 經由 Channel 發布訊息.

message exchange patterns (MEPs) = ExchangeType.Direct

原始碼: 
https://github.com/github-honda/MassTransitPratice/tree/main/Net48/2016Lab/V2016/RabbitMQClient1 

若執行成功的話, 則瀏覽 http://localhost:15672, 可以看到
1. Queues 底下多了一列資料: Virtual host=accounting, Name=my.first.queue.
1.1 在這列資料的後面 Messages.Total 可以看到已接收的訊息數.
1.1.1 點選 (my.first.queue).Get messages 可以檢視訊息內容.
2. Exchanges.Bindings 多了一列資料: To=my.first.queue. 

□ Part 4. 單向接收訊息
Messaging with RabbitMQ and .NET review part 4: one way messaging with a basic consumer
https://dotnetcodr.com/2016/08/08/messaging-with-rabbitmq-and-net-review-part-4-one-way-messaging-with-a-basic-consumer/
示範接收 Onew-way messages
1. 存放在 RabbitMQ 中的訊息, 若接受到 acknowledgement 回覆, 則會從 Queue 中刪除, 否則會持續保留.
2. 若在 RabbitMQ 管理介面檢視過的訊息, 則該訊息的 (redelivered) 屬性為 true.

原始碼: 
https://github.com/github-honda/MassTransitPratice/tree/main/Net48/2016Lab/V2016/ReceiverOneWayMessage


□ Part 5. 以事件通知方式接收訊息
Messaging with RabbitMQ and .NET review part 5: one way messaging with an event based consumer
https://dotnetcodr.com/2016/08/10/messaging-with-rabbitmq-and-net-review-part-5-one-way-messaging-with-an-event-based-consumer/
示範以事件通知接收到訊息: 將(Part 4. Receiver1)改為訊息通知.
原始碼: 
https://github.com/github-honda/MassTransitPratice/tree/main/Net48/2016Lab/V2016/Receiver1Event

□ Part 6. 多點傳輸
Messaging with RabbitMQ and .NET review part 6: the fanout exchange type
https://dotnetcodr.com/2016/08/15/messaging-with-rabbitmq-and-net-review-part-6-the-fanout-exchange-type/

Fanout exchange, 可將一個訊息, 同時發佈到多個 queue.
Fanout exchange can be multiple queues bound to an exchange.
接收的方式, 跟先前示範的內容幾乎相同, 只是從不同的 queue 來源例如接收訊息.

message exchange patterns (MEPs) = ExchangeType.Fanout

原始碼, Fanout Exchange Publisher:
https://github.com/github-honda/MassTransitPratice/tree/main/Net48/2016Lab/V2016/Publisher2
原始碼, 接收 queue= mycompany.queues.accounting
https://github.com/github-honda/MassTransitPratice/tree/main/Net48/2016Lab/V2016/Receiver2acc
原始碼, 接收 queue=mycompany.queues.management
https://github.com/github-honda/MassTransitPratice/tree/main/Net48/2016Lab/V2016/Receiver2Mng


□ Part 7. 雙向通訊
Messaging with RabbitMQ and .NET review part 7: two way messaging
https://dotnetcodr.com/2016/08/18/messaging-with-rabbitmq-and-net-review-part-7-two-way-messaging/

Publisher (Two-way messsaging):
1. Build (ReplyTo queue) and (correlationId) for consumer to response.
2. Publish message with (ReplyTo queue) and (correlationId).
3. Waiting response from (ReplyTo queue).
4. re-publish message with (ReplyTo queue) and (correlationId).
5. Repeat 3 and 4.
原始碼:
https://github.com/github-honda/MassTransitPratice/tree/main/Net48/2016Lab/V2016/Publisher3

Consumer (Two-way messsaging):
1. Receive message including (ReplyTo queue) and (correlationId).
2. Acknowledge the message.
3. Publish response message with (correlationId) to (ReplyTo queue).
原始碼:
Response to (ReplyTo) with (CorrelationId).
https://github.com/github-honda/MassTransitPratice/tree/main/Net48/2016Lab/V2016/Receiver3

Remote procedure calls (RPC)
RPC is slightly different from the previous MEPs in that there’s a response queue involved. The sender sends an initial message to a destination queue via the default exchange. The message properties include a temporary queue where the consumer can reply. The response queue will be dynamically created by the sender. The receiver processes the message and responds using the response queue extracted from the message properties. The sender then processes the response. In this scenario the publisher will also need a consumer class so that it can process the responses. Both parties will also need to acknowledge the messages they receive. Hence the publisher will acknowledge the response from the sender. It’s important to note that we’ll have two queues: a “normal” durable queue where the publisher can send messages to the receiver and then a temporary one where the receiver can send the response. The receiver will be listening on the fixed queue and the publisher on the temporary one.
Note that this setup is not mandatory for two way messaging. You can use a dedicated exchange to route the messages. Moreover, the response queue can be a fixed queue. Hence the usage of the default nameless exchange, called the “(AMQP default)” in the management GUI and the temporary response queue are not obligatory. However, it is probably not necessary to have a dedicated exchange for this purpose and it’s good to know how to use default exchange as well. Furthermore, the usage of temporary queues which disappear after all channels using it are closed is also important knowledge.
We’ve been working with a demo console application in Visual Studio in this series and will continue to do so. We currently have a console project in it that includes all code for the sender. We’ll build upon that to set up the publisher in the RPC scenario.
遠程過程調用（RPC）
RPC與先前的MEP略有不同，因為涉及到一個響應隊列。發送者通過默認交換發送初始消息到目的地隊列。消息屬性包括一個臨時隊列，用於接收方回復。響應隊列將由發送者動態創建。接收者處理消息並使用從消息屬性中提取的響應隊列進行回復。然後發送者處理回應。在這種情況下，發布者還需要一個消費者類，以便它可以處理響應。雙方還需要確認接收到的消息。因此，發布者將確認來自發送者的回應。重要的是要注意，我們將擁有兩個隊列：一個“正常”的持久隊列，發布者可以向接收者發送消息，然後是一個臨時隊列，接收者可以發送響應。接收者將在固定隊列上進行監聽，發布者在臨時隊列上進行監聽。
請注意，此設置對於雙向消息並不是必需的。您可以使用專用交換機來路由消息。此外，響應隊列可以是一個固定的隊列。因此，默認的無名交換機，稱為管理GUI中的“（AMQP默認）”，以及臨時響應隊列的使用並非強制性的。但是，對於此目的，可能不需要專用交換機，了解如何使用默認交換機也是很好的。此外，臨時隊列的使用，這些隊列在所有使用它的通道關閉後會消失，也是重要的知識。
在這個系列中，我們一直在使用Visual Studio中的演示控制台應用程序，並將繼續使用它。我們目前在其中有一個控制台項目，其中包含發送者的所有代碼。我們將在此基礎上構建，設置RPC場景中的發布者。

□ Part 8. 路由鍵 和 主題
Messaging with RabbitMQ and .NET review part 8: routing and topics
https://dotnetcodr.com/2016/08/25/messaging-with-rabbitmq-and-net-review-part-8-routing-and-topics/

○ Routing Key:
message exchange patterns (MEPs) = ExchangeType.Direct

Queue 可以 Bind 綁定為不同的(Exchange + Routing Keys)訊息存放位置.
因此接收一個 queue 的訊息時, 就可以接收來自不同 (Exchange + Routing Keys) 的訊息來源.
The same queue can be bound to an exchange with multiple routing keys. 
i.e., consumer may receive messages from a queue with different binds (exchanges and routing keys).

◇ Publisher (Routing key):
原始碼:
https://github.com/github-honda/MassTransitPratice/tree/main/Net48/2016Lab/V2016/Publisher4Routing

◇ Consumer (Routing key):
原始碼:
https://github.com/github-honda/MassTransitPratice/tree/main/Net48/2016Lab/V2016/Receiver4Routing

○ Topics:
message exchange patterns (MEPs) = ExchangeType.Topic

Queue 可以 Bind 綁定為不同的(Exchange + Topics)訊息存放位置.
因此接收一個 queue 的訊息時, 就可以接收來自不同 (Exchange + Topics) 的訊息來源.
Topic 還可以特殊字元 ‘*’, ‘#’, 過濾符合條件的主題, 例如:
"*.world" = 以 world 結束的2個單字, 以句點分隔.
"world.#" = 以 world 開始的多個單字, 以句點分隔. 
The same queue can be bound to an exchange with multiple Topics. 
i.e., consumer may receive messages from a queue with different binds (exchanges and Topics).
Topics can include special characters as a expression to match:
‘*’ to replace one word
‘#’ to replace 0 or more words


◇ Publisher (Topics):
原始碼:
https://github.com/github-honda/MassTransitPratice/tree/main/Net48/2016Lab/V2016/Publisher4Routing

◇ Consumer (Topics):
原始碼:
https://github.com/github-honda/MassTransitPratice/tree/main/Net48/2016Lab/V2016/Receiver4Routing


□ Part 9. 標頭設定值
Messaging with RabbitMQ and .NET review part 9: headers
https://dotnetcodr.com/2016/08/29/messaging-with-rabbitmq-and-net-review-part-9-headers/

Headers 的用法跟 Topics 類似, 經由設定 html headers "x-match" 屬性為 all 或 any 來過濾符合條件的 header:
Queue 可以 Bind 綁定為不同的(Exchange + Headers)訊息存放位置.

message exchange patterns (MEPs) = ExchangeType.Headers

Headers 分2種 all 和 any:
all:  
    // category=animal and type=mammal
	Dictionary<string, object> headerOptionsWithAll = new Dictionary<string, object>();
	headerOptionsWithAll.Add("x-match", "all");
	headerOptionsWithAll.Add("category", "animal");
	headerOptionsWithAll.Add("type", "mammal");
	channel.QueueBind(sQueue, sExchange, "", headerOptionsWithAll);

any:
    // category=plant or type=tree
	Dictionary<string, object> headerOptionsWithAny = new Dictionary<string, object>();
	headerOptionsWithAny.Add("x-match", "any");
	headerOptionsWithAny.Add("category", "plant");
	headerOptionsWithAny.Add("type", "tree");
	channel.QueueBind(sQueue, sExchange, "", headerOptionsWithAny);

原始碼:
https://github.com/github-honda/MassTransitPratice/tree/main/Net48/2016Lab/V2016/Publisher6Header
https://github.com/github-honda/MassTransitPratice/tree/main/Net48/2016Lab/V2016/Receiver6Header

□ Part 10. 分散/收集 (scatter/gather) 雙向多點通訊
Messaging with RabbitMQ and .NET review part 10: scatter/gather
https://dotnetcodr.com/2016/09/01/messaging-with-rabbitmq-and-net-review-part-10-scattergather/
(Scatter/Gather) 的工作模式類似 (two-way messaging), 但是可以接收不同來源 consumers 的資料.
It is similar to two-way messaging but the publisher will get the responses from multiple consumers.

任一種 message exchange patterns (MEPs) 都可以實作分散/收集 (scatter/gather)的通訊方式.
本範例僅是以 message exchange patterns (MEPs) = ExchangeType.Fanout 示範.

分散/收集 Scatter/Gather, 原是 CPU DMA 處理功能概念: 
DMA是所有現代電腦的重要特色，它允許不同速度的硬體裝置來溝通，而不需要依於中央處理器的大量中斷負載。否則，中央處理器需要從來源把每一片段的資料複製到暫存器，然後把它們再次寫回到新的地方。在這個時間中，中央處理器對於其他的工作來說就無法使用。
DMA傳輸常使用在將一個記憶體區從一個裝置複製到另外一個。當中央處理器初始化這個傳輸動作，傳輸動作本身是由DMA控制器來實行和完成。典型的例子就是移動一個外部記憶體的區塊到晶片內部更快的記憶體去。像是這樣的操作並沒有讓處理器工作拖延，使其可以被重新排程去處理其他的工作。DMA傳輸對於高效能嵌入式系統演算法和網路是很重要的。 舉個例子，個人電腦的ISA DMA控制器擁有8個DMA通道，其中的7個通道是可以讓計算機的中央處理器所利用。每一個DMA通道有一個16位元位址暫存器和一個16位元計數暫存器。要初始化資料傳輸時，裝置驅動程式一起設定DMA通道的位址和計數暫存器，以及資料傳輸的方向，讀取或寫入。然後指示DMA硬體開始這個傳輸動作。當傳輸結束的時候，裝置就會以中斷的方式通知中央處理器。
"分散-收集"（Scatter-gather）DMA允許在一次單一的DMA處理中傳輸資料到多個記憶體區域。相當於把多個簡單的DMA要求串在一起。同樣，這樣做的目的是要減輕中央處理器的多次輸出輸入中斷和資料複製任務。 DRQ意為DMA要求；DACK意為DMA確認。這些符號一般在有DMA功能的電腦系統硬體概要上可以看到。它們表示了介於中央處理器和DMA控制器之間的電子訊號傳輸線路。

原始碼:
Publisher:
  1. 綁定多個 Queue 繫結到同一個 Exchange 同時開放連線.
  2. 接收回應時, 可檢查 CorrelationId 確定來源.
  重點: 測試結果發現 responses List 會記住 3 個 queue 回應的(共 3筆訊息)!
  這代表本函數始終保持執行在記憶體中, 持續等待(Event接收到3個訊息)
  https://github.com/github-honda/MassTransitPratice/tree/main/Net48/2016Lab/V2016/Publisher7ScatterGather

Consumer: 
  接收訊息並回應訊息後就關閉 Connetion, 即可空出 queue 給其他人使用.
  因此測試方式為: 
     將本程式複製為3個, 分別改寫為在接收訊息後, 回應三個不同的訊息到 ReplyTo queue. 
     同時執行3個程式測試.
  https://github.com/github-honda/MassTransitPratice/tree/main/Net48/2016Lab/V2016/Receiver7ScatterGather


□ Part 11. various other topics
https://dotnetcodr.com/2016/09/05/messaging-with-rabbitmq-and-net-review-part-11-various-other-topics/
Messaging with RabbitMQ and .NET review part 11: various other topics
1. Missing queue in publisher.
2. Confirmation from the exchange.
	a. ConfirmSelects: it activates feedback mechanism for the publisher
	b. The BasicAcks event handler which is called in case the message broker has acknowledged the message from the publisher
	c. The BasicNacks event handler which is triggered in case RabbitMq for some reason could not acknowledge a message. In this case you can re-send a message if it’s of critical importance
3. Unacknowledged messages.
原始碼:
https://github.com/github-honda/MassTransitPratice/tree/main/Net48/2016Lab/V2016/Publisher8ConfirmSelect
https://github.com/github-honda/MassTransitPratice/tree/main/Net48/2016Lab/V2016/Publisher8MissingQueue


#### 2014 RabbitMQ.NET Original series  
https://dotnetcodr.com/messaging/
原版 Original series 使用 RabbitMqService 1.0.3 只出到 2019, 已不支援 .net framework 4.8, 
因此改用新版 RabbitMQ.Client 6.4.0.

原始碼:
https://github.com/github-honda/MassTransitPratice/tree/main/Net48/2016Lab/V2016/VOriginal

□ Messaging with RabbitMQ and .NET C# part 3: message exchange patterns

○ MEP One way messaging, 單向發佈訊息
原始碼, RabbitMqService:
https://github.com/github-honda/MassTransitPratice/tree/main/Net48/2016Lab/V2016/VOriginal/Service1
原始碼, OneWayMessageSender:
https://github.com/github-honda/MassTransitPratice/tree/main/Net48/2016Lab/V2016/VOriginal/Publisher1
原始碼, OneWayMessageReceiver:
https://github.com/github-honda/MassTransitPratice/tree/main/Net48/2016Lab/V2016/VOriginal/Consumer1

○ MEP Worker queues, 單向多點發佈訊息
原始碼, RabbitMqService:
https://github.com/github-honda/MassTransitPratice/tree/main/Net48/2016Lab/V2016/VOriginal/Service1
原始碼, WorkerQueueSender:
https://github.com/github-honda/MassTransitPratice/tree/main/Net48/2016Lab/V2016/VOriginal/Publisher2
原始碼, WorkerQueueReceiverOne:
https://github.com/github-honda/MassTransitPratice/tree/main/Net48/2016Lab/V2016/VOriginal/Consumer2A
原始碼, WorkerQueueReceiverTwo:
https://github.com/github-honda/MassTransitPratice/tree/main/Net48/2016Lab/V2016/VOriginal/Consumer2B

○ Publish/Subscribe, 單向多點訂閱訊息
原始碼, RabbitMqService:
https://github.com/github-honda/MassTransitPratice/tree/main/Net48/2016Lab/V2016/VOriginal/Service1
原始碼, PublishSubscribeSender:
https://github.com/github-honda/MassTransitPratice/tree/main/Net48/2016Lab/V2016/VOriginal/Publisher3
原始碼, PublishSubscribeReceiverOne:
https://github.com/github-honda/MassTransitPratice/tree/main/Net48/2016Lab/V2016/VOriginal/Consumer3A
原始碼, PublishSubscribeReceiverTwo:
https://github.com/github-honda/MassTransitPratice/tree/main/Net48/2016Lab/V2016/VOriginal/Consumer3B


○ Remote Procedure (RPC)


Messaging with RabbitMQ and .NET C# part 4: routing and topics
Messaging with RabbitMQ and .NET C# part 5: headers and scatter/gather
RabbitMQ in .NET: data serialisation
RabbitMQ in .NET: data serialisation II
RabbitMQ in .NET: handling large messages
RabbitMQ in .NET C#: basic error handling in Receiver
RabbitMQ in .NET C#: more complex error handling in the Receiver


#### 安裝 MassTransit1 RabbitMQ Docker
□ 安裝 RabbitMQ Docker
這是由 MassTransit 維護的 RabbitMQ Docker, 包括管理介面以及其他 plug-in.
Run RabbitMQ
This is running the preconfigured Docker image maintained by the MassTransit team (opens new window). 
The container image includes the delayed exchange plug-in and the Management interface enabled.
$ docker run -p 15672:15672 -p 5672:5672 --name masstransit1 masstransit/rabbitmq 

  -p, --publish list                   Publish a container's port(s) to
                                       the host
15672 is the default port for RabbitMQ GUI, 
5672 for RabbitMQ message broker.

若未加參數 --name, 則每次執行就會新增一個新的(自動命名)的 container:
$ docker run -p 15672:15672 -p 5672:5672 masstransit/rabbitmq

○ If you are running on an ARM platform
$ docker run --platform linux/arm64 -p 15672:15672 -p 5672:5672 masstransit/rabbitmq

□ 測試 RabbitMQ Docker
登入 rabbitmq broker
安裝成功後, 可瀏覽網頁到 http://localhost:15672 測試登入.
登入帳號guest, 密碼guest, 這組預設帳密只能在本機 localhost 使用.
15672 is the default port for RabbitMQ GUI, 
5672 for RabbitMQ message broker.

或是用指令測試: 
docker 必須已經啟動執行中.


#### RabbitMQ Docker 常用指令
□ 測試 RabbitMQ Docker
$ docker run -p 15672:15672 -p 5672:5672 --name MassTransit1 masstransit/rabbitmq  | 下載由 MassTransit 維護的 RabbitMQ Docker, 並命名為 MassTransit1
-p, --publish list                   Publish a container's port(s) to the host
15672 is the default port for RabbitMQ GUI, 
5672 for RabbitMQ message broker.
安裝後可以網址: http://localhost:15672/, 瀏覽 RabbitMQ 的管理介面.
登入帳號密碼, 預設均為 guest.
guest 帳號除非開啟權限, 否則只能以 localhost 連線使用.
"guest" user can only connect from localhost 


$ docker exec [DockerImageName] rabbitmqctl rabbitmq-plugins enable rabbitmq_management | 開啟 RabbitMQ 管理介面.

$ docker exec [DockerImageName] rabbitmqctl status | 檢查 RabbitMQ Docker Image 的狀態.
例如:
$ docker exec MassTransit1 rabbitmqctl status
Status of node rabbit@1979db74238c ...
Runtime

OS PID: 15
OS: Linux
Uptime (seconds): 1072
Is under maintenance?: false
RabbitMQ version: 3.10.5
Node name: rabbit@1979db74238c
Erlang configuration: Erlang/OTP 24 [erts-12.3.2.2] [source] [64-bit] [smp:8:8] [ds:8:8:10] [async-threads:1] [jit:no-native-stack]
Crypto library: OpenSSL 1.1.1p  21 Jun 2022
Erlang processes: 412 used, 1048576 limit
Scheduler run queue: 1
Cluster heartbeat timeout (net_ticktime): 60

Plugins

Enabled plugin file: /etc/rabbitmq/enabled_plugins
Enabled plugins:

 * rabbitmq_prometheus
 * rabbitmq_shovel_management
 * rabbitmq_shovel
 * amqp10_client
 * prometheus
 * rabbitmq_delayed_message_exchange
 * rabbitmq_consistent_hash_exchange
 * accept
 * rabbitmq_management
 * amqp_client
 * rabbitmq_web_dispatch
 * cowboy
 * cowlib
 * rabbitmq_management_agent

Data directory

Node data directory: /var/lib/rabbitmq/mnesia/rabbit@1979db74238c
Raft data directory: /var/lib/rabbitmq/mnesia/rabbit@1979db74238c/quorum/rabbit@1979db74238c

Config files

 * /etc/rabbitmq/conf.d/10-defaults.conf

Log file(s)

 * /var/log/rabbitmq/rabbit@1979db74238c_upgrade.log
 * <stdout>

Alarms

(none)

Memory

Total memory used: 0.1593 gb
Calculation strategy: rss
Memory high watermark setting: 0.4 of available memory, computed to: 3.267 gb

reserved_unallocated: 0.0852 gb (53.5 %)
code: 0.0394 gb (24.74 %)
other_system: 0.0284 gb (17.82 %)
other_proc: 0.0191 gb (11.98 %)
other_ets: 0.0033 gb (2.09 %)
atom: 0.0015 gb (0.92 %)
plugins: 0.0014 gb (0.9 %)
metrics: 2.0e-4 gb (0.15 %)
mgmt_db: 2.0e-4 gb (0.13 %)
mnesia: 1.0e-4 gb (0.06 %)
binary: 1.0e-4 gb (0.05 %)
msg_index: 0.0 gb (0.02 %)
quorum_ets: 0.0 gb (0.01 %)
connection_other: 0.0 gb (0.0 %)
quorum_queue_dlx_procs: 0.0 gb (0.0 %)
stream_queue_procs: 0.0 gb (0.0 %)
stream_queue_replica_reader_procs: 0.0 gb (0.0 %)
allocated_unused: 0.0 gb (0.0 %)
connection_channels: 0.0 gb (0.0 %)
connection_readers: 0.0 gb (0.0 %)
connection_writers: 0.0 gb (0.0 %)
queue_procs: 0.0 gb (0.0 %)
queue_slave_procs: 0.0 gb (0.0 %)
quorum_queue_procs: 0.0 gb (0.0 %)
stream_queue_coordinator_procs: 0.0 gb (0.0 %)

File Descriptors

Total: 2, limit: 1048479
Sockets: 0, limit: 943629

Free Disk Space

Low free disk space watermark: 0.05 gb
Free disk space: 253.7747 gb

Totals

Connection count: 0
Queue count: 0
Virtual host count: 1

Listeners

Interface: [::], port: 15672, protocol: http, purpose: HTTP API
Interface: [::], port: 15692, protocol: http/prometheus, purpose: Prometheus exporter API over HTTP
Interface: [::], port: 25672, protocol: clustering, purpose: inter-node and CLI tool communication
Interface: [::], port: 5672, protocol: amqp, purpose: AMQP 0-9-1 and AMQP 1.0


#### 相關指令
$ dotnet --list-sdks
6.0.405 [C:\Program Files\dotnet\sdk]
7.0.102 [C:\Program Files\dotnet\sdk]


#### **** 以下確認後移到上面
□ 原始碼:
○ .NET 6 .NETCore
  官網提供的 Sample code (in-memory transport 和 RabbitMQ) 為 Version 8 的 .NETCore 原始碼 
  開發時需要 .NET 6 SDK, 執行時需要 .NET 6.0 以上.
  Service 端建議使用這個版本, .NETCore 程式可以移植到不同的平台, 別再留戀舊版本.

○ .NET 4.x 
ref: 
https://github.com/andras-nemes/messaging-with-mass-transit-introduction
https://github.com/github-honda/MassTransitPratice/blob/main/NET461/messaging-with-mass-transit-introduction-master.zip

○ .NET 4.8
https://github.com/SpocWeb/RabbitMqConsumer.Cli

本文的原始碼版本.
由於 MassTransit 歷史悠久, 版本太多, 各版本教學介紹的廢話也太多 XD..., 
若將錯誤版本的原始碼, 執行在不正確的環境上, 只會狀況百出!
筆者決定直接將(本文原始碼與官網最新的原始碼程式庫)結合, 避免踩雷, 省掉確認不同版本支援程度的驗證時間.
---> 感謝 GitHub !!!

本文的原始碼與官網的原始碼程式庫:
◇ 

目錄 Net48 將 .NET 4.x 的版本, 轉為 .NET 4.8, 並可搭配使用於(官網的.NET 6.測試環境) 
https://github.com/github-honda/MassTransitPratice/tree/main/Net48



RabbitMq Docker Command:
docker pull rabbitmq:3-management
docker run --rm -d -p 15672:15672 -p 5672:5672 --name my_rabbit rabbitmq:3-management
docker stop my_rabbit


The output should have changed to show the message consumer generating the output (again, press Control+C to exit).
https://masstransit-project.com/getting-started/

□ 安裝

在 Windows .net 只安裝測試 rabbitmq docker 建議先閱讀這篇:
https://code.imaginesoftware.it/rabbitmq-with-docker-on-windows-in-30-minutes-172e88bb0808
有概念以後, 再改由 MassTransit 維護的 RabbitMQ docker image 安裝, 比較快適用於 MassTransit with RabbitMQ 的需求.

○ 若是安裝由 MassTransit 維護的 RabbitMQ docker image 最快, 
    https://github.com/MassTransit/Sample-GettingStarted
    https://github.com/MassTransit/Sample-GettingStarted#install-rabbitmq
  則輸入以下指令, 會下載(已經設定好MassTransit 及 RabbitMQ 的)docker image, 並命名 Container name 為 mass1.
    $ docker run -p 15672:15672 -p 5672:5672 --name mass1 masstransit/rabbitmq
    docker run -d -p 8080:80 --restart=always --name nginx nginx	
  注意:
1. 若未加參數 --name, 則會新增一個新的(自動命名)的 container, 結果是每次執行就會新增一個新的(自動命名)的 container. 
2. 15672 is the default port for RabbitMQ GUI, 
3. 5672 for RabbitMQ message broker.


 
下載並啟動: 
docker run -d -p 15672:15672 -p 5672:5672 --name masstransitrabbitmq
或用 uuid
docker run -d -p 15672:15672 -p 5672:5672 --name fd941e3979ee90ae4bfe8cb8751eea10d8ff746d46e400f73c80663b2bfb0a58
-d, --detach                         Run container in background and print container ID
-p, --publish list                   Publish a container's port(s) to the host
-p: mapping RabbitMQ ports to Docker container ports.
    15672 is the default port for RabbitMQ GUI, 
    5672 for RabbitMQ message broker.

docker: Error response from daemon: Conflict. The container name

啟動補充:
若多加了 ImageName, 則是會重新安裝, 當然若 ImageName 已存在的話, 則會提醒 Conflict container name...
啟動: docker run -d -p 15672:15672 -p 5672:5672 --name masstransitrabbitmq masstransit/rabbitmq
或用 uuid
docker run -d -p 15672:15672 -p 5672:5672 --name fd941e3979ee90ae4bfe8cb8751eea10d8ff746d46e400f73c80663b2bfb0a58 masstransit/rabbitmq

?????
https://joshhu.gitbooks.io/dockercommands/content/Containers/DockerRunMore.html
	


Docker attached vs detached mode
https://www.java4coding.com/contents/docker/docker-attached-vs-detached-mode
You can run container in attached mode (in the foreground) or in detached mode (in the background). 

Docker 預設以 attached mode 啟動 container 內的處理緒, 並將 處理緒的 standard input, standard output, and standard error 導出.
By default, Docker runs the container in attached mode. In the attached mode, 
Docker can start the process in the container and attach the console to the process’s standard input, standard output, and standard error. 

若 Docker 以 Detached mode (--detach or –d flag)啟動, 則會以背景程式方式執行, 且不會導出 standard input, standard output, and standard error.
Detached mode 下, 不需要停止 container, 就可以開關 terminal session.
Detached mode, started by the option --detach or –d flag in docker run command, means that a Docker container runs in the background of your terminal. 
It does not receive input or display output. 
Using detached mode also allows you to close the opened terminal session without stopping the container. 
docker run -d [docker_image]

While running a container in the foreground is that you cannot access the command prompt anymore, as you can see from the screenshot below. 
Which means you cannot run any other commands while the container is running. 
In the first screenshot we run in detached mode, in the second screenshot we run in attached mode.
detached mode: 
docker run -d --name springbootappcontainer -p 8080:8080 springbootapp
docker-attached-vs-detached-mode-0

attached mode:
docker run --name springbootappcontainer -p 8080:8080 springbootapp
docker-attached-vs-detached-mode-1

docker attach
To reattach to a detached container, use docker attach command.
docker attach <nameofcontainer>
OR
docker attach <dockerid>



○ 若是安裝 RabbitMQ 維護的 docker image, 
參考這連結: https://code.imaginesoftware.it/rabbitmq-with-docker-on-windows-in-30-minutes-172e88bb0808
則輸入指令為:
docker pull rabbitmq:3-management
最後 "rabbitmq:3-management" 為 docker image name.

再啟動 RabbitMQ Docker image
docker run -d -p 15672:15672 -p 5672:5672 --name rabbit-test-for-medium rabbitmq:3-management
-p: mapping RabbitMQ ports to Docker container ports.
15672 is the default port for RabbitMQ GUI, 
5672 for RabbitMQ message broker.

-name: giving a name to our container, in order to identify it in a more readable way than using the generated GUID.
最後 "rabbitmq:3-management" 為 docker image name.

○ 瀏覽 RabbitMQ 管理網頁 
瀏覽網址: http:localhost:15672 或 http://localhost:15672/#/
以上2種網址均會導引到 http://localhost:15672/#/

預設帳號密碼均為 guest.
guest 帳號預設只能以 localhost 連線使用.
"guest" user can only connect from localhost 

docker exec rabbitmq rabbitmqctl status
docker exec fd941e3979ee rabbitmqctl status
docker exec funny_cerf rabbitmqctl status

測試 RabbitMQ docker:

◇ 測試 Port 可否開啟?
RabbitMQ 預設 port 為 5672.
例如:
telnet localhost 5672  <--- 若 Port 可開啟, 則會等待輸入, 因此可隨意輸入任何值, 在看 Server 怎麼回應. 
telnet localhost 5673  <--- 若 Port 不可開啟, 則Teltnet 會結束執行, 並顯示連線到主機Port失敗.

◇ 測試資料內容
工具: TcpDump, WinDump 或 Wireshark. 
https://www.rabbitmq.com/amqp-wireshark.html
  
◇ TLS Connections 
https://www.rabbitmq.com/troubleshooting-ssl.html 




□ MassTransit vs RabbitMQ: What are the differences?
https://stackshare.io/stackups/masstransit-vs-rabbitmq
MassTransit: 
Lightweight message bus for creating distributed applications. 
MassTransit is free software/open-source .NET-based Enterprise Service Bus software that helps Microsoft developers route messages over MSMQ, RabbitMQ, TIBCO and ActiveMQ service busses, with native support for MSMQ and RabbitMQ; 

RabbitMQ: 
A messaging broker - an intermediary for messaging. RabbitMQ gives your applications a common platform to send and receive messages, and your messages a safe place to live until received.

MassTransit and RabbitMQ belong to "Message Queue" category of the tech stack.
Some of the features offered by MassTransit are:
	Message-based communication
	Reliable
	Scalable

On the other hand, RabbitMQ provides the following key features:
	Robust messaging for applications
	Easy to use
	Runs on all major operating systems
	RabbitMQ is an open source tool with 6.07K GitHub stars and 1.85K GitHub forks. Here's a link to RabbitMQ's open source repository on GitHub.				

□ Things that MT adds on top of just using RabbitMQ.
https://stackoverflow.com/questions/12296787/what-does-masstransit-add-to-rabbitmq

○ Optimized, asynchronous multithreaded, concurrent consumers
○ Message serialization, with support for interfaces, classes, and records, including guidance on versioning message contracts
○ Automatic exchange bindings, publish conventions
○ Saga state machines, including persistent state via Entity Framework Core, MongoDB, Redis, etc.
○ Built-in metrics, Open Telemetry, Prometheus
○ Message headers
○ Fault handling, message retry, message redelivery

Those are just a few, some more significant than others. 
The fact that the bus hosts your consumers, handlers, sagas, and manages all of the threading is probably the biggest advantage, and the fact that you can host multiple buses in the same process.

Serialization is the next biggest benefit, since that can be painful to figure out, and getting an interface-based message contract with automatic deserialized into types (including dynamically-backed interface types) is huge. Publishing a single class that implements multiple interfaces, and seeing all interested consumers pick up their piece of the message asynchronously is just awesome in production as new interfaces can be added to producers and down-level consumers are unaffected.

Those are a few, you can check out the documentation for more information, or give the really old .NET Rocks! podcast a listen for some related content by yours truly.


2023-01-18, rabbitmq-with-docker-on-windows-in-30-minutes
以下為
https://code.imaginesoftware.it/rabbitmq-with-docker-on-windows-in-30-minutes-172e88bb0808
的文字節錄: (包含C#測試程式)

檔案可參考: CodeHelper\cs\MessageQueue\MassTransit\RabbitMQ with Docker on Windows in 30.pdf

RabbitMQ with Docker on Windows in 30 minutes
How to set up a RabbitMQ instance with Docker on Windows and start sending messages.

Want to quickly spin up RabbitMQ on your Windows development environment? Maybe you just want to try the most deployed message broker and you don’t want to try it on Azure or ask your IT crew to deploy a new server just for that: Docker is your solution.

Install Docker for Windows: just download it and let the installer do the rest
Start Docker and wait for its the initialization. You will see an icon of a whale in the taskbar
Open Powershell and type:


If you don’t see any errors, and you see a Docker version, it is correctly installed.

4. Now we have to download the RabbitMQ image. In this case, we will download the image with a management plugin in order to be able to view the RabbitMQ server-side GUI. The command to download or update a Docker image is pull



5. Now we will start the RabbitMQ Docker image with a simple command:


With the p argument we are mapping RabbitMQ ports to Docker container ports. 15672 is the default port for RabbitMQ GUI, 5672 for RabbitMQ message broker. With the name argument we are giving a name to our container, in order to identify it in a more readable way than using the generated GUID. This will allow us to easily stop, remove, and manage our containers. At the end, we specify the docker image to run, in this case the one we pulled before.

If the operation goes well, you will see a GUID as output.

6. Test the image by opening “http://localhost:15672/#/” in your browser. The default login is guest guest: you should see the RabbitMQ management GUI.


The RabbitMQ management GUI hosted by Docker
If you see this page correctly, the most important part is done!

7. Now we are ready to send messages to our RabbitMQ endpoint! In this phase the choice is yours: I work daily with .NET so I will show you a C# example. Just to remind you, the default values to connect to the endpoint with your preferred language:

HostName: “localhost”
UserName: “guest”
Password: “guest”
Port: 5672
In my case, I will create a new Windows Console Application, install RabbitMQ.Client Nuget package and write some lines of code to create a queue and send a message every 500 milliseconds:
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace TestRabbit
{
    internal static class Program
    {
        static async Task Main(string[] args)
        {
            const string queueName = "testqueue";

            try
            {
                var connectionFactory = new ConnectionFactory()
                {
                    HostName = "localhost",
                    UserName = "guest",
                    Password = "guest",
                    Port = 5672,
                    RequestedConnectionTimeout = 3000, // milliseconds
                };

                using (var rabbitConnection = connectionFactory.CreateConnection())
                {
                    using (var channel = rabbitConnection.CreateModel())
                    {
                        // Declaring a queue is idempotent 
                        channel.QueueDeclare(
                            queue: queueName,
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

                        while (true)
                        {
                            string body = $"A nice random message: {DateTime.Now.Ticks}";
                            channel.BasicPublish(
                                exchange: string.Empty,
                                routingKey: queueName,
                                basicProperties: null,
                                body: Encoding.UTF8.GetBytes(body));

                            Console.WriteLine("Message sent");
                            await Task.Delay(500);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.WriteLine("End");
            Console.Read();
        }
    }
}

In this case, I’m using a little cheat related to RabbitMQ: I’m sending messages directly to the queue without setting a binding to an exchange, by setting the routing key of the publish method with the same name as the queue I declared.

8. At the end, we want to be sure that the messages are correctly received by the queue. We can just open our management GUI, click on Queues navigation button, then testqueue. If we are sending messages we will see the chart's lines moving:


RabbitMQ management GUI charts when sending messages to a queue
Also, you can click on the Get messages menu to grab a message from the queue and read its body:


The body of a RabbitMQ message in our example
I don’t have the experience to say how Docker is on production environments, but in test/study environments, I think that Docker is really a time saving infrastructure!



2023-01-18
    https://github.com/dprothero/MtPubSubExample
	https://masstransit-project.com/usage/transports/
	https://masstransit-project.com/usage/transports/rabbitmq.html#minimal-example
	https://www.nuget.org/packages/MassTransit.RabbitMQ/

	********
	https://github.com/MassTransit/Sample-GettingStarted
	https://github.com/MassTransit/MassTransit


	********
	https://github.com/andras-nemes/messaging-with-mass-transit-introduction
	https://dotnetcodr.com/messaging/
	https://dotnetcodr.com/2016/09/08/messaging-through-a-service-bus-in-net-using-masstransit-part-1-foundations/
	Messaging through a service bus in .NET using MassTransit part
	Exercises in .NET with Andras Nemes
