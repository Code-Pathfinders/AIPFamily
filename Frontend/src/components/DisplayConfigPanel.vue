<template>
  <div class="config-panel" :class="{ collapsed: isCollapsed }">
    <div class="panel-header" @click="toggleCollapse">
      <h3>⚙️ 显示配置</h3>
      <span class="toggle-icon">{{ isCollapsed ? '▶' : '▼' }}</span>
    </div>
    
    <div v-show="!isCollapsed" class="panel-content">
      <!-- 布局方向配置 -->
      <div class="config-section">
        <h4>布局方向</h4>
        <div class="layout-options">
          <label class="radio-option">
            <input 
              type="radio" 
              :value="'TB'" 
              v-model="localConfig.layoutDirection"
              @change="emitChange"
            />
            <span>⬇️ 自上而下</span>
          </label>
          <label class="radio-option">
            <input 
              type="radio" 
              :value="'BT'" 
              v-model="localConfig.layoutDirection"
              @change="emitChange"
            />
            <span>⬆️ 自下而上</span>
          </label>
          <label class="radio-option">
            <input 
              type="radio" 
              :value="'LR'" 
              v-model="localConfig.layoutDirection"
              @change="emitChange"
            />
            <span>➡️ 从左到右</span>
          </label>
          <label class="radio-option">
            <input 
              type="radio" 
              :value="'RL'" 
              v-model="localConfig.layoutDirection"
              @change="emitChange"
            />
            <span>⬅️ 从右到左</span>
          </label>
        </div>
      </div>

      <!-- 卡片字段配置 -->
      <div class="config-section">
        <h4>卡片显示字段</h4>
        <div class="field-options">
          <label class="checkbox-option">
            <input 
              type="checkbox" 
              v-model="localConfig.cardFields.showName"
              @change="emitChange"
            />
            <span>显示姓名</span>
          </label>
          <label class="checkbox-option">
            <input 
              type="checkbox" 
              v-model="localConfig.cardFields.showGeneration"
              @change="emitChange"
            />
            <span>显示世代</span>
          </label>
          <label class="checkbox-option">
            <input 
              type="checkbox" 
              v-model="localConfig.cardFields.showBirthYear"
              @change="emitChange"
            />
            <span>显示出生年份</span>
          </label>
          <label class="checkbox-option">
            <input 
              type="checkbox" 
              v-model="localConfig.cardFields.showGender"
              @change="emitChange"
            />
            <span>显示性别</span>
          </label>
          <label class="checkbox-option">
            <input 
              type="checkbox" 
              v-model="localConfig.cardFields.showAvatar"
              @change="emitChange"
            />
            <span>显示头像</span>
          </label>
        </div>
      </div>

      <!-- 节点尺寸配置 -->
      <div class="config-section">
        <h4>节点尺寸</h4>
        <div class="size-options">
          <div class="size-input">
            <label>宽度：</label>
            <input 
              type="number" 
              v-model.number="localConfig.nodeWidth"
              @change="emitChange"
              min="100"
              max="300"
            />
            <span>px</span>
          </div>
          <div class="size-input">
            <label>高度：</label>
            <input 
              type="number" 
              v-model.number="localConfig.nodeHeight"
              @change="emitChange"
              min="60"
              max="200"
            />
            <span>px</span>
          </div>
        </div>
      </div>

      <!-- 间距配置 -->
      <div class="config-section">
        <h4>间距设置</h4>
        <div class="size-options">
          <div class="size-input">
            <label>节点间距：</label>
            <input 
              type="number" 
              v-model.number="localConfig.nodeSpacing"
              @change="emitChange"
              min="20"
              max="200"
            />
            <span>px</span>
          </div>
          <div class="size-input">
            <label>层级间距：</label>
            <input 
              type="number" 
              v-model.number="localConfig.rankSpacing"
              @change="emitChange"
              min="50"
              max="300"
            />
            <span>px</span>
          </div>
        </div>
      </div>

      <!-- 连线样式配置 -->
      <div class="config-section">
        <h4>🔗 连线样式</h4>
        <div class="edge-style-options">
          <div class="edge-group">
            <h5>父子关系线</h5>
            <div class="edge-config">
              <div class="color-input">
                <label>颜色：</label>
                <input 
                  type="color" 
                  v-model="localConfig.edgeStyle.fatherLineColor"
                  @change="emitChange"
                />
                <span>{{ localConfig.edgeStyle.fatherLineColor }}</span>
              </div>
              <div class="style-select">
                <label>样式：</label>
                <select v-model="localConfig.edgeStyle.fatherLineStyle" @change="emitChange">
                  <option value="solid">━━ 实线</option>
                  <option value="dashed">┈┈ 虚线</option>
                </select>
              </div>
            </div>
          </div>

          <div class="edge-group">
            <h5>母子关系线</h5>
            <div class="edge-config">
              <div class="color-input">
                <label>颜色：</label>
                <input 
                  type="color" 
                  v-model="localConfig.edgeStyle.motherLineColor"
                  @change="emitChange"
                />
                <span>{{ localConfig.edgeStyle.motherLineColor }}</span>
              </div>
              <div class="style-select">
                <label>样式：</label>
                <select v-model="localConfig.edgeStyle.motherLineStyle" @change="emitChange">
                  <option value="solid">━━ 实线</option>
                  <option value="dashed">┈┈ 虚线</option>
                </select>
              </div>
            </div>
          </div>

          <div class="edge-common">
            <div class="size-input">
              <label>线宽：</label>
              <input 
                type="number" 
                v-model.number="localConfig.edgeStyle.lineWidth"
                @change="emitChange"
                min="1"
                max="5"
                step="0.5"
              />
              <span>px</span>
            </div>
            <label class="checkbox-option">
              <input 
                type="checkbox" 
                v-model="localConfig.edgeStyle.showShadow"
                @change="emitChange"
              />
              <span>显示阴影</span>
            </label>
          </div>
        </div>
      </div>

      <!-- 操作按钮 -->
      <div class="config-actions">
        <button class="btn btn-primary" @click="applyConfig">应用配置</button>
        <button class="btn btn-default" @click="resetConfig">重置默认</button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue'
import type { GraphDisplayConfig } from '@/types'

/**
 * 组件属性
 */
interface Props {
  config: GraphDisplayConfig
}

const props = defineProps<Props>()

/**
 * 事件定义
 */
const emit = defineEmits<{
  change: [config: GraphDisplayConfig]
  apply: [config: GraphDisplayConfig]
}>()

// 本地配置状态
const localConfig = ref<GraphDisplayConfig>({ ...props.config })
const isCollapsed = ref(false)

/**
 * 切换折叠状态
 */
const toggleCollapse = () => {
  isCollapsed.value = !isCollapsed.value
}

/**
 * 发出配置变化事件
 */
const emitChange = () => {
  emit('change', { ...localConfig.value })
}

/**
 * 应用配置
 */
const applyConfig = () => {
  emit('apply', { ...localConfig.value })
}

/**
 * 重置为默认配置
 */
const resetConfig = () => {
  localConfig.value = {
    layoutDirection: 'TB',
    cardFields: {
      showName: true,
      showGeneration: true,
      showBirthYear: true,
      showGender: false,
      showAvatar: true
    },
    nodeWidth: 150,
    nodeHeight: 80,
    nodeSpacing: 50,
    rankSpacing: 100,
    edgeStyle: {
      fatherLineColor: '#5B8FF9',
      motherLineColor: '#F759AB',
      lineWidth: 2.5,
      fatherLineStyle: 'solid',
      motherLineStyle: 'dashed',
      showShadow: true
    }
  }
  emitChange()
}

// 监听外部配置变化
watch(() => props.config, (newConfig) => {
  localConfig.value = { ...newConfig }
}, { deep: true })
</script>

<style scoped>
.config-panel {
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  overflow: hidden;
  transition: all 0.3s;
}

.config-panel.collapsed {
  width: 200px;
}

.panel-header {
  padding: 16px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  cursor: pointer;
  display: flex;
  justify-content: space-between;
  align-items: center;
  user-select: none;
}

.panel-header h3 {
  margin: 0;
  font-size: 16px;
  font-weight: 600;
}

.toggle-icon {
  font-size: 12px;
  transition: transform 0.3s;
}

.panel-content {
  padding: 16px;
  max-height: calc(100vh - 200px);
  overflow-y: auto;
}

.config-section {
  margin-bottom: 20px;
  padding-bottom: 16px;
  border-bottom: 1px solid #e8e8e8;
}

.config-section:last-of-type {
  border-bottom: none;
}

.config-section h4 {
  margin: 0 0 12px 0;
  font-size: 14px;
  font-weight: 600;
  color: #333;
}

.layout-options,
.field-options {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.radio-option,
.checkbox-option {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 8px;
  border-radius: 4px;
  cursor: pointer;
  transition: background 0.2s;
}

.radio-option:hover,
.checkbox-option:hover {
  background: #f5f5f5;
}

.radio-option input,
.checkbox-option input {
  cursor: pointer;
}

.radio-option span,
.checkbox-option span {
  font-size: 14px;
  color: #666;
}

.size-options {
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.size-input {
  display: flex;
  align-items: center;
  gap: 8px;
}

/* 连线样式配置 */
.edge-style-options {
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.edge-group {
  padding: 12px;
  background: #f8f9fa;
  border-radius: 6px;
}

.edge-group h5 {
  margin: 0 0 10px 0;
  font-size: 13px;
  font-weight: 600;
  color: #555;
}

.edge-config {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.color-input {
  display: flex;
  align-items: center;
  gap: 8px;
}

.color-input input[type="color"] {
  width: 40px;
  height: 30px;
  border: 1px solid #d9d9d9;
  border-radius: 4px;
  cursor: pointer;
}

.color-input span {
  font-size: 12px;
  color: #999;
  font-family: monospace;
}

.style-select {
  display: flex;
  align-items: center;
  gap: 8px;
}

.style-select select {
  flex: 1;
  padding: 6px 10px;
  border: 1px solid #d9d9d9;
  border-radius: 4px;
  font-size: 13px;
  cursor: pointer;
}

.edge-common {
  display: flex;
  flex-direction: column;
  gap: 10px;
  padding: 12px;
  background: #f0f0f0;
  border-radius: 6px;
}

.size-input label {
  font-size: 13px;
  color: #666;
  min-width: 80px;
}

.size-input input {
  flex: 1;
  padding: 6px 10px;
  border: 1px solid #d9d9d9;
  border-radius: 4px;
  font-size: 13px;
}

.size-input span {
  font-size: 12px;
  color: #999;
}

.config-actions {
  display: flex;
  gap: 8px;
  margin-top: 16px;
}

.btn {
  flex: 1;
  padding: 10px 16px;
  border: none;
  border-radius: 4px;
  font-size: 14px;
  cursor: pointer;
  transition: all 0.3s;
}

.btn-primary {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
}

.btn-primary:hover {
  opacity: 0.9;
  transform: translateY(-1px);
}

.btn-default {
  background: #f5f5f5;
  color: #666;
}

.btn-default:hover {
  background: #e8e8e8;
}

/* 滚动条样式 */
.panel-content::-webkit-scrollbar {
  width: 6px;
}

.panel-content::-webkit-scrollbar-track {
  background: #f1f1f1;
  border-radius: 3px;
}

.panel-content::-webkit-scrollbar-thumb {
  background: #888;
  border-radius: 3px;
}

.panel-content::-webkit-scrollbar-thumb:hover {
  background: #555;
}
</style>
