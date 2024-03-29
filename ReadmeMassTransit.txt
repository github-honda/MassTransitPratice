From: 011netservice@gmail.com
Date: 2023-03-20
Subject: MassTransit, RabbitMQ 原始碼範例

章節:
#### .NETCore 範例
#### .Net Framework 範例
#### 安裝 MassTransit1 RabbitMQ Docker
#### RabbitMQ Docker 常用指令

歡迎來信交流, 訂購軟體需求.

MassTransit:
  https://masstransit.io/
  https://masstransit-project.com/
  https://github.com/MassTransit/MassTransit
  https://dotnetcodr.com/messaging/
 
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

#### .Net Framework 範例
請參考 https://github.com/github-honda/MassTransitPratice/blob/main/AndrasNemes/Readme-MassTransitRabbitMQ.txt

#### 安裝 MassTransit1 RabbitMQ Docker

安裝 RabbitMQ Docker 比較簡單, 不需要安裝 RabbitMQ 到主 Host.

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
