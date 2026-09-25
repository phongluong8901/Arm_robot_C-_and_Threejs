import * as THREE from 'three';
import { OrbitControls } from 'three/addons/controls/OrbitControls.js';
import './style.css';

const scene = new THREE.Scene();
scene.background = new THREE.Color(0x111820);
scene.fog = new THREE.Fog(0x111820, 20, 42);

const camera = new THREE.PerspectiveCamera(45, innerWidth / innerHeight, 0.1, 100);
camera.position.set(12, 10, 14);

const renderer = new THREE.WebGLRenderer({ antialias: true, powerPreference: 'high-performance' });
renderer.setPixelRatio(Math.min(devicePixelRatio, 2));
renderer.setSize(innerWidth, innerHeight);
renderer.shadowMap.enabled = true;
renderer.shadowMap.type = THREE.PCFSoftShadowMap;
renderer.outputColorSpace = THREE.SRGBColorSpace;
document.querySelector('#app').appendChild(renderer.domElement);

const controls = new OrbitControls(camera, renderer.domElement);
controls.target.set(0, 0.8, 0);
controls.enableDamping = true;
controls.maxPolarAngle = Math.PI * 0.47;
controls.minDistance = 7;
controls.maxDistance = 28;

const materials = {
  floor: new THREE.MeshStandardMaterial({ color: 0x2f3842, roughness: 0.88 }),
  machine: new THREE.MeshStandardMaterial({ color: 0x37434d, roughness: 0.72, metalness: 0.25 }),
  dark: new THREE.MeshStandardMaterial({ color: 0x232c34, roughness: 0.78, metalness: 0.3 }),
  blue: new THREE.MeshStandardMaterial({ color: 0x287ba8, roughness: 0.5, metalness: 0.35 }),
  red: new THREE.MeshStandardMaterial({ color: 0xd45348, roughness: 0.55, metalness: 0.25 }),
  yellow: new THREE.MeshStandardMaterial({ color: 0xefc45d, roughness: 0.58 }),
  orange: new THREE.MeshStandardMaterial({ color: 0xef8354, roughness: 0.65 }),
  cyan: new THREE.MeshStandardMaterial({ color: 0x63c6d8, emissive: 0x10323b, emissiveIntensity: 0.5 }),
};

const cube = (size, material, position) => {
  const mesh = new THREE.Mesh(new THREE.BoxGeometry(...size), material);
  mesh.position.set(...position);
  mesh.castShadow = true;
  mesh.receiveShadow = true;
  scene.add(mesh);
  return mesh;
};

const floor = cube([22, 0.1, 16], materials.floor, [0, -0.1, 0]);
const grid = new THREE.GridHelper(22, 22, 0x4c5863, 0x3d4852);
grid.position.y = -0.03;
scene.add(grid);

// Conveyor body, belt surface and guide rails.
cube([18, 0.45, 2.8], materials.machine, [0, 0.35, 0]);
cube([17.6, 0.12, 2.45], materials.dark, [0, 0.62, 0]);
cube([18.1, 0.12, 0.16], materials.cyan, [0, 0.79, -1.18]);
cube([18.1, 0.12, 0.16], materials.cyan, [0, 0.79, 1.18]);
cube([0.25, 1.2, 3.1], materials.dark, [-8.9, 0.9, 0]);
cube([0.25, 1.2, 3.1], materials.dark, [8.9, 0.9, 0]);

for (let x = -8; x <= 8; x += 1.25) {
  cube([0.08, 0.05, 2.2], materials.cyan, [x, 0.71, 0]);
}

const box = cube([0.95, 0.5, 0.95], materials.orange, [-7.5, 0.98, 0]);
const boxEdges = new THREE.LineSegments(
  new THREE.EdgesGeometry(new THREE.BoxGeometry(0.95, 0.5, 0.95)),
  new THREE.LineBasicMaterial({ color: 0x3b2822 })
);
box.add(boxEdges);

// Six-axis industrial arm: base rotation, two arm joints and three wrist axes.
const robot = new THREE.Group();
robot.position.set(0, 0, -3.1);
scene.add(robot);
const axisAngles = [0, 0, 0, 0, 0, 0];
const axisGroups = [];
const makePart = (geometry, material, position, parent) => {
  const part = new THREE.Mesh(geometry, material);
  part.position.set(...position);
  part.castShadow = true;
  part.receiveShadow = true;
  parent.add(part);
  return part;
};
const joint = (parent, position) => {
  const group = new THREE.Group();
  group.position.set(...position);
  parent.add(group);
  axisGroups.push(group);
  makePart(new THREE.SphereGeometry(0.35, 18, 12), materials.orange, [0, 0, 0], group);
  return group;
};
makePart(new THREE.CylinderGeometry(0.95, 0.95, 0.55, 24), materials.blue, [0, 0.85, 0], robot);
const axis1 = joint(robot, [0, 1.15, 0]);
makePart(new THREE.CylinderGeometry(0.58, 0.58, 0.45, 20), materials.blue, [0, 0, 0], axis1);
const axis2 = joint(axis1, [0, 0.25, 0]);
makePart(new THREE.BoxGeometry(0.58, 0.38, 2.35), materials.red, [0, 0, 1.15], axis2);
const axis3 = joint(axis2, [0, 0, 2.3]);
makePart(new THREE.BoxGeometry(0.48, 0.32, 1.9), materials.yellow, [0, 0, 0.95], axis3);
const axis4 = joint(axis3, [0, 0, 1.9]);
makePart(new THREE.CylinderGeometry(0.3, 0.3, 0.55, 18), materials.blue, [0, 0, 0], axis4);
const axis5 = joint(axis4, [0, 0.32, 0]);
makePart(new THREE.BoxGeometry(0.42, 0.7, 0.42), materials.red, [0, 0.35, 0], axis5);
const axis6 = joint(axis5, [0, 0.7, 0]);
makePart(new THREE.CylinderGeometry(0.2, 0.2, 0.5, 16), materials.yellow, [0, 0.25, 0], axis6);
makePart(new THREE.BoxGeometry(0.55, 0.18, 0.3), materials.dark, [0, 0.55, 0], axis6);

const station = new THREE.Group();
station.position.set(5.5, 1.1, -3.2);
station.add(cube([2.2, 2.2, 1.2], materials.dark, [5.5, 1.1, -3.2]));
const screen = cube([1.3, 0.8, 0.12], materials.cyan, [5.5, 2.45, -3.2]);
screen.material = materials.cyan;

const ambientLight = new THREE.HemisphereLight(0xa9c7d2, 0x182027, 2.2);
scene.add(ambientLight);
const keyLight = new THREE.DirectionalLight(0xffe6c2, 3.2);
keyLight.position.set(5, 13, 7);
keyLight.castShadow = true;
keyLight.shadow.mapSize.set(2048, 2048);
scene.add(keyLight);
const fillLight = new THREE.PointLight(0x4ec6df, 18, 16);
fillLight.position.set(-6, 5, -5);
scene.add(fillLight);

let running = true;
let boxPosition = -7.5;
let robotTime = 0;
let processed = 0;
let lastTimestamp = performance.now();
let fpsAccumulator = 0;
let fpsFrames = 0;
let fpsTimer = 0;
let speed = 1;
let autoMode = true;
let emergencyStop = false;

const statusText = document.querySelector('#status-text');
const statusDot = document.querySelector('#status-dot');
const toggleButton = document.querySelector('#toggle-button');
const boxCount = document.querySelector('#box-count');
const fpsText = document.querySelector('#fps');
const modeButton = document.querySelector('#mode-button');
const modeLabel = document.querySelector('#robot-mode-label');
const estopButton = document.querySelector('#estop-button');
const axisList = document.querySelector('#axis-list');

for (let index = 0; index < 6; index += 1) {
  const row = document.createElement('label');
  row.className = 'axis-row';
  row.innerHTML = `<span>J${index + 1}</span><input data-axis="${index}" type="range" min="-180" max="180" value="0" /><output>0°</output>`;
  axisList.appendChild(row);
  row.querySelector('input').addEventListener('input', (event) => {
    axisAngles[index] = Number(event.target.value) * Math.PI / 180;
    row.querySelector('output').textContent = `${event.target.value}°`;
  });
}

function setRunning(nextValue) {
  running = nextValue && !emergencyStop;
  statusText.textContent = running ? 'DANG CHAY' : 'TAM DUNG';
  statusDot.classList.toggle('paused', !running);
  toggleButton.textContent = running ? 'Tam dung' : 'Tiep tuc';
}

function resetSimulation() {
  boxPosition = -7.5;
  robotTime = 0;
  processed = 0;
  boxCount.textContent = '0';
  emergencyStop = false;
  estopButton.classList.remove('active');
  setRunning(true);
}

toggleButton.addEventListener('click', () => setRunning(!running));
document.querySelector('#reset-button').addEventListener('click', resetSimulation);
modeButton.addEventListener('click', () => {
  autoMode = !autoMode;
  modeButton.textContent = autoMode ? 'Manual' : 'Auto';
  modeLabel.textContent = autoMode ? 'AUTO' : 'MANUAL';
});
estopButton.addEventListener('click', () => {
  emergencyStop = !emergencyStop;
  estopButton.classList.toggle('active', emergencyStop);
  setRunning(!emergencyStop);
});
document.querySelector('#speed-input').addEventListener('input', (event) => {
  speed = Number(event.target.value);
  document.querySelector('#speed-value').textContent = `${speed.toFixed(2)}x`;
});
addEventListener('keydown', (event) => {
  if (event.code === 'Space') {
    event.preventDefault();
    setRunning(!running);
  }
  if (event.code === 'KeyR') resetSimulation();
});

function updateRobot() {
  if (autoMode && !emergencyStop) {
    axisAngles[0] = Math.sin(robotTime * 1.1) * 0.55;
    axisAngles[1] = Math.sin(robotTime * 1.7) * 0.35;
    axisAngles[2] = Math.cos(robotTime * 1.7) * 0.45;
    axisAngles[3] = Math.sin(robotTime * 2.1) * 0.8;
    axisAngles[4] = Math.cos(robotTime * 1.4) * 0.5;
    axisAngles[5] = Math.sin(robotTime * 2.8) * 1.1;
  }
  axisGroups[0].rotation.y = axisAngles[0];
  axisGroups[1].rotation.x = axisAngles[1];
  axisGroups[2].rotation.x = axisAngles[2];
  axisGroups[3].rotation.z = axisAngles[3];
  axisGroups[4].rotation.x = axisAngles[4];
  axisGroups[5].rotation.z = axisAngles[5];
}

function animate(timestamp) {
  const delta = Math.min((timestamp - lastTimestamp) / 1000, 0.05);
  lastTimestamp = timestamp;

  if (running) {
    boxPosition += delta * 2.2;
    if (boxPosition > 7.5) boxPosition = -7.5;
    robotTime += delta * speed;
    if (robotTime >= 4.2) {
      robotTime = 0;
      processed += 1;
      boxCount.textContent = String(processed);
    }
  }

  box.position.x = boxPosition;
  updateRobot();
  controls.update();
  renderer.render(scene, camera);

  fpsAccumulator += 1 / Math.max(delta, 0.001);
  fpsFrames += 1;
  fpsTimer += delta;
  if (fpsTimer > 0.5) {
    fpsText.textContent = String(Math.round(fpsAccumulator / fpsFrames));
    fpsAccumulator = 0;
    fpsFrames = 0;
    fpsTimer = 0;
  }

  requestAnimationFrame(animate);
}

addEventListener('resize', () => {
  camera.aspect = innerWidth / innerHeight;
  camera.updateProjectionMatrix();
  renderer.setSize(innerWidth, innerHeight);
});

requestAnimationFrame(animate);
