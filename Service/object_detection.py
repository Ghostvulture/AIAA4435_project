import cv2
import mrcnn.config
import mrcnn.utils
from mrcnn.model import MaskRCNN

class InferenceConfig(mrcnn.config.Config):
    NAME = "coco_inference"
    GPU_COUNT = 1
    IMAGES_PER_GPU = 1
print("1")
inference_config = InferenceConfig()
model = MaskRCNN(mode="inference", config=inference_config, model_dir="./")

print("11")
model.load_weights("./mask_rcnn_balloon.h5", by_name=True)

print("111")
image_path = "./The_Great_Wave.jpg"
image = cv2.imread(image_path)
image = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)

results = model.detect([image], verbose=0)
r = results[0]

class_names = ["surfboard","boat","mountain"]
wave_class_id = class_names.index("surfboard")  # Replace with the appropriate class name
boats_class_id = class_names.index("boat")
mount_fuji_class_id = class_names.index("mountain")

wave_mask = r['masks'][:, :, r['class_ids'] == wave_class_id]
boats_mask = r['masks'][:, :, r['class_ids'] == boats_class_id]
mount_fuji_mask = r['masks'][:, :, r['class_ids'] == mount_fuji_class_id]

highlighted_wave = cv2.bitwise_and(image, image, mask=wave_mask[:, :, 0])
highlighted_boats = cv2.bitwise_and(image, image, mask=boats_mask[:, :, 0])
highlighted_mount_fuji = cv2.bitwise_and(image, image, mask=mount_fuji_mask[:, :, 0])

import matplotlib.pyplot as plt

plt.figure(figsize=(15, 5))

plt.subplot(1, 3, 1)
plt.imshow(highlighted_wave)
plt.title("Highlighted Wave")

plt.subplot(1, 3, 2)
plt.imshow(highlighted_boats)
plt.title("Highlighted Boats")

plt.subplot(1, 3, 3)
plt.imshow(highlighted_mount_fuji)
plt.title("Highlighted Mount Fuji")

plt.tight_layout()
plt.show()