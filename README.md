# rabbitFanout
projeto estudo de exchange do tipo Fanout do RabbitMQ, no qual uma mensagem pode ser entregue para multiplos consumidores

## rodar do rabbit local com docker
executar no prompt: docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.10-management
https://www.rabbitmq.com/download.html


## documentação
https://www.rabbitmq.com/tutorials/tutorial-three-dotnet.html

## rodar consumidores
ir até a pasta do executavel e rodar o comando substituindo {queueName} pelo nome da fila que deseja rodar:
Consumer.exe "{queueName}"
