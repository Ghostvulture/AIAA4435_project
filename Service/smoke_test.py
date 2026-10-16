import threading, urllib.request, json
from werkzeug.serving import make_server
from chatgpt_service import app
server = make_server('127.0.0.1', 0, app)
thread = threading.Thread(target=server.serve_forever, daemon=True)
thread.start()
try:
    with urllib.request.urlopen(f'http://127.0.0.1:{server.server_port}/chatgpt/status') as response:
        assert json.load(response) == {'status': 'ok'}
    print('Real HTTP health check: PASS')
finally:
    server.shutdown()
    thread.join()
